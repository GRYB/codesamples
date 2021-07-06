using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Client.Audio.FMODExtras;
using Client.SceneManagement;
using Client.SceneManagement.ScreenFader;
using GameData;
using Gameplay;
using Gameplay.Cards;
using Messaging;
using Networking.Messages;
using UI.Data;
using UI.Messages;
using UI.Views;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIShopMediator : UIScreenTutorialViewMediator
    {
        #region constants

        private const int TutorialActionTransitionDelay = 2;
        private readonly int[] TutorialCardsIds = { 60, 105, 57, 48, 126, 115, 158, 129 };

        #endregion

        private List<BandMemberData> _members = new List<BandMemberData>();
        private BandMemberData _selectedBandMember;
        private int? _selectedCardIndex;

        private BandAssembleCallBacks _callBacks;

        private int _currentTutorialStep = 0;
        private bool _tutorialCardIsSelected;

        public override ViewType ViewKey => ViewType.DiscographyShopScreen;
        public override TutorialView TutorialView => _screen.TutorialView;

        public override void OnTutorialActionButtonClicked()
        {
            _screen.TutorialView.ActivateTutorialButton(false);
            TutorialActionHandle(++_currentTutorialStep);
        }

        public override void ShowView(UIViewPreloadData preloadData = null)
        {
            base.ShowView(preloadData);

            SubscribeEvents();

            GameSceneManagersContainer.Instance.BattleManager.SetChangingStateBlocked(true);
            _screen.InitializeTutorial();
            StartCoroutine(IntroduceView());
        }

        public override void HideView()
        {
            UnSubscribeEvents();
            _screen.UnInitializeTutorial();
            base.HideView();

            foreach (var member in AppManager.GameData.Members)
            {
                member.EquipedAccessories?.Clear();
            }

            _screen.Hide();
            _screen = null;
        }

        public override void Receive(object sender, IMessage message)
        {
            switch (message)
            {
                case InterruptBattleMessage msg:
                    OnInterruptBattle(msg);
                    break;
                case RefreshBandMemberValuesMessage msg:
                    OnRefreshBandMemberValuesMessage();
                    break;
                case GoldChangedMessage msg:
                    OnGoldChangedMessage(msg);
                    break;
            }
        }

        #region Tutorial

        private void StartTutorial()
        {
            GameSceneManagersContainer.Instance.BattleManager.SetChangingStateBlocked(true);
            TutorialActionHandle(_currentTutorialStep);
        }

        private IEnumerator IntroduceView()
        {
            _screen.TutorialView.ShowIntroTutorialView(_screen.TutorialView.IntroViewDuration);
            yield return new WaitForSeconds(_screen.TutorialView.IntroViewDuration);
            StartTutorial();
        }

        private void SetPlayerInputActive(bool isActive)
        {
            _screen.SetPlayerInputActive(isActive);
        }

        IEnumerator TutorialActionDelayedTransition()
        {
            yield return new WaitForSeconds(TutorialActionTransitionDelay);
            TutorialActionHandle(++_currentTutorialStep);
        }

        private void SkipTutorial()
        {
            AppManager.TutorialManager.SkipTutorial();
        }

        private void SubscribeEvents()
        {
            _screen.OnTutorialActionButtonClicked += OnTutorialActionButtonClicked;
            _screen.OnTutorialSkipButtonClicked += SkipTutorial;
        }

        private void UnSubscribeEvents()
        {
            _screen.OnTutorialActionButtonClicked -= OnTutorialActionButtonClicked;
            _screen.OnTutorialSkipButtonClicked -= SkipTutorial;
        }

        #endregion

        #region Shared Shop logic

        private void OnSelectedBandPanel_SelectBandMember(BandMemberData bandMemberData, SelectBandViewModeType type)
        {
            if (bandMemberData.Instrument == TutorialManager.TutorialBandMemberInstrument)
            {
                _selectedCardIndex = null;
                _screen.InitializeSelectedPreviewBandMember(_callBacks, bandMemberData, GetCardOfSelectedBandMember());
            }
        }
        #endregion
    }
}