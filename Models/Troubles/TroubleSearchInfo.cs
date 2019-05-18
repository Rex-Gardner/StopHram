namespace Models.Troubles
{
    public class TroubleSearchInfo
    {
        /// <summary>
        /// Позиция, начиная с которой нужно производить поиск
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Количество проблем, которое нужно вернуть
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Теги
        /// </summary>
        public string[] Tags { get; set; }
        
        /// <summary>
        /// Статусы
        /// </summary>
        public TroubleStatus?[] Statuses { get; set; }
    }
}