namespace RapidApi.WebUserInterfaceLayer.Models.MovieViewModels
{
    public class MovieViewModel
    {

            public int Rank { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Image { get; set; }
            public string Big_image { get; set; }
            public string[] Genre { get; set; }
            public string Thumbnail { get; set; }
            public string Rating { get; set; }
            public string Id { get; set; }
            public int Year { get; set; }
            public string Imdbid { get; set; }
            public string Imdb_link { get; set; }
    }
}