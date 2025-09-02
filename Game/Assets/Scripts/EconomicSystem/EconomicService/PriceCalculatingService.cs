using System;
using Scripts.Systems;
using UnityEngine;

namespace Project.Service.EconomicService
{
    public class PriceCalculatingService
    {
        /// <summary>
        /// Calculates the final price of an item considering both the player's reputation and the item's rarity.
        /// </summary>
        /// <param name="itemPrice">The base price of the item.</param>
        /// <param name="reputationCoefficient">The coefficient based on the player's reputation.</param>
        /// <param name="itemRarityCoefficient">The coefficient based on the item's rarity.</param>
        /// <returns>The final price of the item, rounded down to the nearest integer.</returns>
        public int CalculateItemPrice(int itemPrice, float reputationCoefficient, float itemRarityCoefficient)
        {
            return ParseToFloat(CalculateItemPrice(itemPrice, itemRarityCoefficient) * reputationCoefficient);
        }

        /// <summary>
        /// Calculates the price of an item based solely on its rarity.
        /// </summary>
        /// <param name="itemPrice">The base price of the item.</param>
        /// <param name="itemRarityCoefficient">The coefficient based on the item's rarity.</param>
        /// <returns>The item price adjusted for rarity, rounded down to the nearest integer.</returns>
        public int CalculateItemPrice(int itemPrice, float itemRarityCoefficient)
        {
            return ParseToFloat(itemPrice * itemRarityCoefficient);
        }

        /// <summary>
        /// Calculates the sell price of an item considering the player's reputation.
        /// </summary>
        /// <param name="itemPrice">The base price of the item.</param>
        /// <param name="reputationCoefficient">The coefficient based on the player's reputation with NPCs.</param>
        /// <param name="itemRarityCoefficient">The coefficient based on the item's rarity.</param>
        /// <returns>The sell price of the item, rounded down to the nearest integer.</returns>
        public int CalculateSellItemPrice(int itemPrice, float reputationCoefficient, float itemRarityCoefficient)
        {
            return ParseToFloat((itemPrice * itemRarityCoefficient) / reputationCoefficient);
        }

        /// <summary>
        /// Converts a floating-point value to an integer by rounding down.
        /// </summary>
        /// <param name="value">The input float value.</param>
        /// <returns>The value rounded down to the nearest integer.</returns>
        private int ParseToFloat(float value)
        {
            return Mathf.FloorToInt(value);
        }
    }
}
