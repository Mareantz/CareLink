namespace Application.AIMLforTuberculosis
{
    public class TuberculosisData
    {
        public float age { get; set; }
        public string gender { get; set; }
        public float weight {  get; set; }

        public string past_TB_history { get; set; }
        public string cough { get; set; }
        public string hemoptysis { get; set; }
        public string fever { get; set; }
        public string night_sweats { get; set; }
	public string weight_loss { get; set; }
        public string close_contact_with_TP_patients { get; set; }
        public string HIV{ get; set; }
        public string positive_TST { get; set; }

        public float risk { get; set; }

    }
}

