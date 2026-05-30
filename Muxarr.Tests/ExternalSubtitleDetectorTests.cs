using Muxarr.Core.Models;
using Muxarr.Core.Utilities;

namespace Muxarr.Tests;

[TestClass]
public class ExternalSubtitleDetectorTests
{
    [TestMethod]
    public void Parse_ThreeLetterLanguage_SetsLanguage()
    {
        var sub = ExternalSubtitleDetector.ParseFromFileName("Elemental", "Elemental.eng.srt");

        Assert.IsNotNull(sub);
        Assert.AreEqual("eng", sub.LanguageCode);
        Assert.AreEqual("English", sub.LanguageName);
        Assert.AreEqual("Srt", sub.Codec);
        Assert.IsFalse(sub.IsForced);
        Assert.IsFalse(sub.IsHearingImpaired);
    }

    [TestMethod]
    public void Parse_TwoLetterLanguage_Resolves()
    {
        var sub = ExternalSubtitleDetector.ParseFromFileName("Elemental", "Elemental.en.srt");
        Assert.IsNotNull(sub);
        Assert.AreEqual("English", sub!.LanguageName);
    }

    [TestMethod]
    public void Parse_ForcedToken_SetsForcedFlag()
    {
        var sub = ExternalSubtitleDetector.ParseFromFileName("Elemental", "Elemental.eng.forced.srt");
        Assert.IsNotNull(sub);
        Assert.AreEqual("English", sub!.LanguageName);
        Assert.IsTrue(sub.IsForced);
    }

    [TestMethod]
    public void Parse_SdhToken_SetsHearingImpairedFlag()
    {
        var sub = ExternalSubtitleDetector.ParseFromFileName("Elemental", "Elemental.eng.sdh.srt");
        Assert.IsNotNull(sub);
        Assert.IsTrue(sub!.IsHearingImpaired);
    }

    [TestMethod]
    public void Parse_BareSubtitle_DefaultsToUndetermined()
    {
        var sub = ExternalSubtitleDetector.ParseFromFileName("Elemental", "Elemental.srt");
        Assert.IsNotNull(sub);
        Assert.AreEqual("und", sub!.LanguageCode);
        Assert.AreEqual("Undetermined", sub.LanguageName);
    }

    [TestMethod]
    public void Parse_AssExtension_MapsCodec()
    {
        var sub = ExternalSubtitleDetector.ParseFromFileName("Elemental", "Elemental.eng.ass");
        Assert.IsNotNull(sub);
        Assert.AreEqual("Ass", sub!.Codec);
    }

    [TestMethod]
    public void Parse_DifferentStem_ReturnsNull()
    {
        var sub = ExternalSubtitleDetector.ParseFromFileName("Elemental", "OtherMovie.eng.srt");
        Assert.IsNull(sub);
    }

    [TestMethod]
    public void Parse_NonSubtitleExtension_ReturnsNull()
    {
        var sub = ExternalSubtitleDetector.ParseFromFileName("Elemental", "Elemental.eng.txt");
        Assert.IsNull(sub);
    }
}
