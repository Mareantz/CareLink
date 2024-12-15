namespace Application.AIML
{
    public class UserData
    {
        public int age { get; set; }
        public string gender { get; set; }

        public float weight {  get; set; }
        public float bloodPressure {  get; set; } //between 60-180
        public float cholesterolLevel {  get; set; }//40-200
        public int physicalActivityLevel {  get; set; } //0, 1 or 2
        public bool smokingStatus {  get; set; }
        public int stressLevel {  get; set; }//0, 1 or 2
        public float risk { get; set; }

    }
}