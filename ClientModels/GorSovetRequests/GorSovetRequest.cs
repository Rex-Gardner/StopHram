namespace ClientModels.GorSovetRequests
{
    public class GorSovetRequest
    {
        //TODO поздравляю, вы нашли самый главный говнокод :)
        //TODO реализовать нормальную модель
        // фамилия
        public string Form_text_6 { get; set; }
        // имя
        public string Form_text_27 { get; set; }
        // отчество 
        public string Form_text_28 { get; set; }
        // возраст 
        public string Form_dropdown_AGE { get; set; }
        // телефон
        public string Form_text_33 { get; set; }
        // email
        public string Form_email_4 { get; set; }
        // текст 
        public string Form_textarea_2 { get; set; }
        // кому 
        public string Form_dropdown_RECIPIENT { get; set; }

        //todo больше пользовательских данных
        public const string ServiceName = "Сервис прямая линия";
        public const string IdkHowToFillIt = "Без отчества";
        public const string MiddleAge = "30";    // это вообще value параметр из списка, который не соотв. значению
        public const string ServicePhone = "8-800-555-35-35";
        public const string ServiceEmail = "test@localhost.ru";
        public const string Recipient = "52";    // тоже value параметр
        
        public GorSovetRequest(string userName, string description)
        {
            Form_text_6 = ServiceName;
            Form_text_27 = userName;
            Form_text_28 = IdkHowToFillIt;
            Form_dropdown_AGE = MiddleAge;
            Form_text_33 = ServicePhone;
            Form_email_4 = ServiceEmail;
            Form_textarea_2 = description;
            Form_dropdown_RECIPIENT = Recipient;
        }
    }
}