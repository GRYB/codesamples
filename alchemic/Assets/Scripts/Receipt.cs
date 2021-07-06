using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Receipt
{
    [SerializeField] int id;
    [SerializeField] string description;
    [SerializeField] List<int> ingredients;
    [SerializeField] Color color;

    public int GetId()
    {
        return id;
    }

    public Color GetColor(){
        return color;
    }

    public Receipt(Receipt receipt){
        this.id = receipt.id;
        this.description = receipt.description;
        this.ingredients = new List<int>(receipt.ingredients);
        this.color = receipt.GetColor();
    }

    public void AddIngredients(int ingredientId)
    {
        ingredients.Add(ingredientId);
    }

    public List<int> GetIngredients()
    {
        return ingredients;
    }

    public bool Contains(int id){
        return ingredients.Contains(id);
    }

    public bool RemoveIngredient(int id){
        if(ingredients.Contains(id)){
            ingredients.Remove(id);
            return true;
        }
        return false;
    }

    public string GetDescription(){
        return description;
    }

    public bool Validate(Receipt reference)
    {
        //reference has correct number of ingrediens
        if (reference.ingredients.Count == this.ingredients.Count)
        {
            foreach (var key in reference.ingredients)
            {
                if (!reference.ingredients.Remove(key))
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
        return true;
    }
}
