using System.Collections.Generic;
using WAW.Models;

namespace WAW.ViewModel
{
    public class HomeIndexViewModel
    {
        public List<Ad> LatestAds { get; set; }
        public List<Ad> MostLikedAds { get; set; }
    }
}
