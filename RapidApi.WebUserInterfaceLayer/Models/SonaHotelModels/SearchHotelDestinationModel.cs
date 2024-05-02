namespace RapidApi.WebUserInterfaceLayer.Models.SonaHotelModels
{
    public class SearchHotelDestinationModel
    {
        public Datum[] data { get; set; }
        public class Datum
        {
            public string dest_id { get; set; }       
            public string name { get; set; }
            public string search_types { get; set; }            
        }
    }
}
