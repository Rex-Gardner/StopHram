namespace ClientModels.Troubles
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
        public string[] Tag { get; set; }
        
        /// <summary>
        /// Статусы
        /// </summary>
        public string[] Status { get; set; }
    }
}