using System;
class Episode
{
    public string season { get; set; }
    public int episodeNumberOverall { get; set; } //EpisodeNumber
    public int episodeNumberInSeason { get; set; } //EpisodeNumber2
    public string title { get; set; }
    public string titleRomaji { get; set; }
    public string titleKanji { get; set; }
    public DateTime originalAirDate { get; set; }

    public Episode(){}

    //English Series Episode Format
    public Episode(string season, int seriesEpisodeNumber, int episodeNumber, string title, DateTime originalAirDate)
    {
        this.season = season;
        this.episodeNumberOverall = seriesEpisodeNumber;
        this.episodeNumberInSeason = episodeNumber;
        this.title = title;
        this.originalAirDate = originalAirDate;

        //Null valued
        this.titleRomaji = null;
        this.titleKanji = null;
    }

    //Japanese Series Episode Format
    public Episode(string season, int seriesEpisodeNumber, int episodeNumber, string title, string titleRomaji, string titleKanji, DateTime originalAirDate)
    {
        this.season = season;
        this.episodeNumberOverall = seriesEpisodeNumber;
        this.episodeNumberInSeason = episodeNumber;
        this.title = title;
        this.titleRomaji = titleRomaji;
        this.titleKanji = titleKanji;
        this.originalAirDate = originalAirDate;
    }


}