namespace RapidApi.WebUserInterfaceLayer.Models.SonaHotelModels
{
    public class SearchHotelViewModel
    {
        public int dest_id { get; set; }
        public string search_type { get; set; }
        public DateTime arrival_date { get; set; }
        public DateTime departure_date { get; set; }
    }
}