using Downloader.Helpers;
using Downloader.Model;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using YoutubeExtractor;

namespace Downloader.ViewModel
{
    public class DownloadPlanViewModel
    {
        #region Members and Properties     

        ObservableCollection<DownloadItem> downloadItems = new ObservableCollection<DownloadItem>();
        public ObservableCollection<DownloadItem> DownloadItems { get; set; }

        #endregion

        public DownloadPlanViewModel()
        {
            DownloadItems = new ObservableCollection<DownloadItem>();
            /*
            int count = 1;
            foreach (var item in TestUrls)
            {
                DownloadItems.Add(new DownloadItem
                {
                    Order = count,
                    Status = "Nothing",
                    Url = item
                });
                count++;
            }
            */
            DownloadItems.Add(new DownloadItem() {
                Order = 2,
                Url = "https://www.youtube.com/watch?v=stodaz1ze_c",
                Status = "Nothing"
            });

            DownloadItems.Add(new DownloadItem()
            {Order = 62, Url = "https://www.youtube.com/watch?v=tgGed4RjdgI", Status = "Nothing"});
            DownloadItems.Add(new DownloadItem()
            { Order = 68, Url = "https://www.youtube.com/watch?v=3csfGENg8zI", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 72, Url = "https://www.youtube.com/watch?v=N4saeAyHZ_k", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 75, Url = "https://www.youtube.com/watch?v=OtLerBBJhO4", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 87, Url = "https://www.youtube.com/watch?v=BXHrg-92DR4", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 85, Url = "https://www.youtube.com/watch?v=pXUwv222quc", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 88, Url = "https://www.youtube.com/watch?v=qsa-FqDXB0g", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 97, Url = "https://www.youtube.com/watch?v=7psJfMIvLlQ", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 102, Url = "https://www.youtube.com/watch?v=6vXAEzvG_fc", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 107, Url = "https://www.youtube.com/watch?v=gDjLHRXtkT4", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 131, Url = "https://www.youtube.com/watch?v=tSgZoYTfpJ8", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 134, Url = "https://www.youtube.com/watch?v=jrfoFC-jCmQ", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 137, Url = "https://www.youtube.com/watch?v=7yUWV7srQeI", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 161, Url = "https://www.youtube.com/watch?v=TbYKezpV6lM", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 176, Url = "https://www.youtube.com/watch?v=M3R8pPX-v-Y", Status = "Nothing" });
            DownloadItems.Add(new DownloadItem()
            { Order = 178, Url = "https://www.youtube.com/watch?v=sO6dmYNB0rI", Status = "Nothing" });

        }

        #region Commands

        async void StartDownload()
        {
            int count = 1;
            foreach (var item in DownloadItems)
            {
                try
                {
                    IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(item.Url, false);

                    VideoInfo video = videoInfos.FirstOrDefault(info => info.VideoType == VideoType.Mp4 && info.Resolution == 720);
                    if (video == null) video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);
                    if (video.RequiresDecryption) DownloadUrlResolver.DecryptDownloadUrl(video);

                    DirectoryInfo target = new DirectoryInfo(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\YoutubeDownloader");
                    if (!target.Exists) target.Create();

                    var videoDownloader = new CustomVideoDownloader(item, video,
                        Path.Combine(
                            target.FullName,
                            $"{item.Order}_{RemoveIllegalPathCharacters(video.Title)}{video.VideoExtension}")
                            );

                    videoDownloader.DownloadProgressChanged += VideoDownloader_DownloadProgressChanged;
                    videoDownloader.Execute();
                } catch (Exception ex)
                {
                    item.Status = "Error";
                    item.Comment = ex.Message;
                }
                count++;
            }

        }

        bool CanStartDownload()
        {
            return true;
        }

        public ICommand StartDownloadPlan { get { return new RelayAsyncCommand(StartDownload, CanStartDownload); } }


        #endregion

        #region TestData

        List<string> TestUrls = new List<string>()
        {
            "https://www.youtube.com/watch?v=S1mCIw7pC_4",
            "https://www.youtube.com/watch?v=stodaz1ze_c",
            "https://www.youtube.com/watch?v=TlX85sEqO_E",
            "https://www.youtube.com/watch?v=Lvh8tfzin2U",
            "https://www.youtube.com/watch?v=64P8y6Kho8Y",
            "https://www.youtube.com/watch?v=vLjZXvK8Vc4",
            "https://www.youtube.com/watch?v=VfW5hxwcMH4",
            "https://www.youtube.com/watch?v=BXldkId3BJI",
            "https://www.youtube.com/watch?v=Ghxyix34YpE&amp;t=330s",
            "https://www.youtube.com/watch?v=ksIDNhgdz2E",
            "https://www.youtube.com/watch?v=zvvFenK5O40",
            "https://www.youtube.com/watch?v=RKYRaYw4smU",
            "https://www.youtube.com/watch?v=ylperMtCjGs",
            "https://www.youtube.com/watch?v=1xWfostjYI4",
            "https://www.youtube.com/watch?v=8_8eyaLutzM",
            "https://www.youtube.com/watch?v=ya9uCoIEVvQ",
            "https://www.youtube.com/watch?v=aoqZi-QvmjM&amp;t=292s",
            "https://www.youtube.com/watch?v=DM1YKYfY_B8",
            "https://www.youtube.com/watch?v=6FR9h9StPnY",
            "https://www.youtube.com/watch?v=a9IgwiJ2FTM",
            "https://www.youtube.com/watch?v=6eFJ0XOFUFM",
            "https://www.youtube.com/watch?v=whSjF05HnMw&amp;t=4s",
            "https://www.youtube.com/watch?v=uWM1FmpAkbQ",
            "https://www.youtube.com/watch?v=H2BlDu_pKwI",
            "https://www.youtube.com/watch?v=cMn3I8kKj80",
            "https://www.youtube.com/watch?v=sVOQDX22Kdw",
            "https://www.youtube.com/watch?v=0UL_GP1qeq8&amp;t=13s",
            "https://www.youtube.com/watch?v=3ngrmfF01zk",
            "https://www.youtube.com/watch?v=ECyDEoFc1WA",
            "https://www.youtube.com/watch?v=r1LfYOle-zg",
            "https://www.youtube.com/watch?v=jq1fyexmCSA",
            "https://www.youtube.com/watch?v=5sElhZtjtYE",
            "https://www.youtube.com/watch?v=ba2g3WH41-c",
            "https://www.youtube.com/watch?v=kOlf2yvmDFk",
            "https://www.youtube.com/watch?v=n3l_OhzcwFM",
            "https://www.youtube.com/watch?v=4DWaaGydgZ0",
            "https://www.youtube.com/watch?v=KzD3DqloN_Q",
            "https://www.youtube.com/watch?v=x0b7nK7LoGw",
            "https://www.youtube.com/watch?v=AoaJ4PxO7es",
            "https://www.youtube.com/watch?v=iLjiudMOZAE",
            "https://www.youtube.com/watch?v=ankTCwydalg",
            "https://www.youtube.com/watch?v=emXmvHM14ZQ",
            "https://www.youtube.com/watch?v=pjF18uzKAJ8",
            "https://www.youtube.com/watch?v=WAlnXDoRinE",
            "https://www.youtube.com/watch?v=5Om5D6Lwc2Q",
            "https://www.youtube.com/watch?v=05WDRZLbElA",
            "https://www.youtube.com/watch?v=0-mTOe69d9o",
            "https://www.youtube.com/watch?v=48GuLCjgLqU",
            "https://www.youtube.com/watch?v=BDw41eIIfSk",
            "https://www.youtube.com/watch?v=SaCzawQq_fg",
            "https://www.youtube.com/watch?v=iXmd1aZ-Cl0",
            "https://www.youtube.com/watch?v=nmONXy28CDs",
            "https://www.youtube.com/watch?v=9IeoSlT38bg&amp;t=77s",
            "https://www.youtube.com/watch?v=OruZRG7Qg5k",
            "https://www.youtube.com/watch?v=ajcBPtSh05c",
            "https://www.youtube.com/watch?v=r70-g3uOSPI",
            "https://www.youtube.com/watch?v=fEl5XHMQ1oQ",
            "https://www.youtube.com/watch?v=qd_fBINRchs",
            "https://www.youtube.com/watch?v=L8WhR103zig",
            "https://www.youtube.com/watch?v=tFPtByzPQm4",
            "https://www.youtube.com/watch?v=gpMMewcE8B8",
            "https://www.youtube.com/watch?v=tgGed4RjdgI&amp;t=74s",
            "https://www.youtube.com/watch?v=oqoralbTeGk",
            "https://www.youtube.com/watch?v=Abj3as2ppeM&amp;t=630s",
            "https://www.youtube.com/watch?v=yuohdMphj0U",
            "https://www.youtube.com/watch?v=sbwTrCHvtds",
            "https://www.youtube.com/watch?v=2OKVtWs_C_g",
            "https://www.youtube.com/watch?v=3csfGENg8zI",
            "https://www.youtube.com/watch?v=5NZCCjoMvJI",
            "https://www.youtube.com/watch?v=qLOpw3gjGZg",
            "https://www.youtube.com/watch?v=OulXiFDU-8Y",
            "https://www.youtube.com/watch?v=N4saeAyHZ_k",
            "https://www.youtube.com/watch?v=kLMrk3FKQ4s",
            "https://www.youtube.com/watch?v=Qyvf1UUOHq4",
            "https://www.youtube.com/watch?v=OtLerBBJhO4",
            "https://www.youtube.com/watch?v=iKgd3xgWARk",
            "https://www.youtube.com/watch?v=-mmHnOePKRI",
            "https://www.youtube.com/watch?v=ibEz0RRS3IQ",
            "https://www.youtube.com/watch?v=jbISt6CTceI",
            "https://www.youtube.com/watch?v=IoqJyXYwq8U&amp;t=533s",
            "https://www.youtube.com/watch?v=GMn9jlv5-uE",
            "https://www.youtube.com/watch?v=hdlFKpteRWQ",
            "https://www.youtube.com/watch?v=moj12VRlTNw",
            "https://www.youtube.com/watch?v=BXHrg-92DR4",
            "https://www.youtube.com/watch?v=pXUwv222quc",
            "https://www.youtube.com/watch?v=nOAGSGE5rXc",
            "https://www.youtube.com/watch?v=MscicnnNWWY",
            "https://www.youtube.com/watch?v=qsa-FqDXB0g",
            "https://www.youtube.com/watch?v=VUY9q4VUdPU",
            "https://www.youtube.com/watch?v=y8s0uG7bU2E&amp;t=140s",
            "https://www.youtube.com/watch?v=JFfu5M6Fh68",
            "https://www.youtube.com/watch?v=S8OHhb-qpS0",
            "https://www.youtube.com/watch?v=DFcj51m6yC4",
            "https://www.youtube.com/watch?v=iT4vWQHgeZ8",
            "https://www.youtube.com/watch?v=EMSL5kEUcRk",
            "https://www.youtube.com/watch?v=_30dRh0QgRE",
            "https://www.youtube.com/watch?v=7psJfMIvLlQ",
            "https://www.youtube.com/watch?v=L5pdRTQpANo",
            "https://www.youtube.com/watch?v=x0eFc1otpaM",
            "https://www.youtube.com/watch?v=Zo5N3_mJ8BY",
            "https://www.youtube.com/watch?v=ybkgQgrmrX4",
            "https://www.youtube.com/watch?v=6vXAEzvG_fc",
            "https://www.youtube.com/watch?v=Pu8jv53_f0s",
            "https://www.youtube.com/watch?v=aWJdZX8inGg",
            "https://www.youtube.com/watch?v=zsOIQfW67iY",
            "https://www.youtube.com/watch?v=ILt-5_KksQI",
            "https://www.youtube.com/watch?v=gDjLHRXtkT4",
            "https://www.youtube.com/watch?v=E8T0CiXoqh4",
            "https://www.youtube.com/watch?v=mRtopJ2n8wY",
            "https://www.youtube.com/watch?v=_4OTktQ80cY",
            "https://www.youtube.com/watch?v=dFPpdyCkU5o",
            "https://www.youtube.com/watch?v=yBvcI1obT-Y",
            "https://www.youtube.com/watch?v=Be4Csiig9E0",
            "https://www.youtube.com/watch?v=urnQc_ImwK0",
            "https://www.youtube.com/watch?v=F5rMGkCQVeA",
            "https://www.youtube.com/watch?v=siqk3xZqA0M",
            "https://www.youtube.com/watch?v=qsTh-OFqAjI",
            "https://www.youtube.com/watch?v=XHpyaAscPIs",
            "https://www.youtube.com/watch?v=z04BUN86jBs",
            "https://www.youtube.com/watch?v=Gc0kSckidjc",
            "https://www.youtube.com/watch?v=iLArbkaauxI",
            "https://www.youtube.com/watch?v=ThrRbcImEHQ",
            "https://www.youtube.com/watch?v=AywZOr4Gw_U",
            "https://www.youtube.com/watch?v=sfrutj83diQ",
            "https://www.youtube.com/watch?v=6fAXIXzyJwQ",
            "https://www.youtube.com/watch?v=FNls8YdEsn0",
            "https://www.youtube.com/watch?v=8eBijfCcq8k",
            "https://www.youtube.com/watch?v=TBzp9x542Lo",
            "https://www.youtube.com/watch?v=IeOfnLvVRH4",
            "https://www.youtube.com/watch?v=Tc-9aOHC-eI",
            "https://www.youtube.com/watch?v=tSgZoYTfpJ8",
            "https://www.youtube.com/watch?v=hp07R8Tbg2A",
            "https://www.youtube.com/watch?v=DLOVHavtnXA",
            "https://www.youtube.com/watch?v=jrfoFC-jCmQ",
            "https://www.youtube.com/watch?v=F3THgldYM7w",
            "https://www.youtube.com/watch?v=tnKJ4W0VEr8",
            "https://www.youtube.com/watch?v=7yUWV7srQeI",
            "https://www.youtube.com/watch?v=BGxvytTWOZw",
            "https://www.youtube.com/watch?v=p4eovvDsqNc",
            "https://www.youtube.com/watch?v=BkiKxnxxCWU",
            "https://www.youtube.com/watch?v=DOgfclYf5WM",
            "https://www.youtube.com/watch?v=0yp8t0shjoc",
            "https://www.youtube.com/watch?v=NoUIE_Y03P8",
            "https://www.youtube.com/watch?v=Jp3eoAQYHMY",
            "https://www.youtube.com/watch?v=fpnkVSfuk5E",
            "https://www.youtube.com/watch?v=ZFLBvSu4eZE",
            "https://www.youtube.com/watch?v=9EnqIPv3Klo",
            "https://www.youtube.com/watch?v=mkKke36v4rE",
            "https://www.youtube.com/watch?v=MFg8RDG6hKg",
            "https://www.youtube.com/watch?v=43bSctYZbL8",
            "https://www.youtube.com/watch?v=b613je3NrKA",
            "https://www.youtube.com/watch?v=wa1MZZJNF0Q",
            "https://www.youtube.com/watch?v=KU6A_tJ91yI",
            "https://www.youtube.com/watch?v=HYVw1nDCxag",
            "https://www.youtube.com/watch?v=aQMgviiifD8",
            "https://www.youtube.com/watch?v=tfFvAdLNA_s",
            "https://www.youtube.com/watch?v=z3d1B2OKzBc",
            "https://www.youtube.com/watch?v=F9mWkR1rFXg",
            "https://www.youtube.com/watch?v=_7ld0VrNgvQ",
            "https://www.youtube.com/watch?v=mLg0bpnv5oE",
            "https://www.youtube.com/watch?v=TbYKezpV6lM",
            "https://www.youtube.com/watch?v=dV7V0kh38ZU",
            "https://www.youtube.com/watch?v=2D08kOiVeQs",
            "https://www.youtube.com/watch?v=S16f2NaAFhw",
            "https://www.youtube.com/watch?v=pwb6LFGudUI",
            "https://www.youtube.com/watch?v=LvPqFwCJRfA",
            "https://www.youtube.com/watch?v=5ooRvTGw6hA",
            "https://www.youtube.com/watch?v=TjHR3SSR4dU",
            "https://www.youtube.com/watch?v=dajtwv_FvKw",
            "https://www.youtube.com/watch?v=Oo6J6MPviHw",
            "https://www.youtube.com/watch?v=XAEaTeWp700",
            "https://www.youtube.com/watch?v=Opvn4Pd7pUs",
            "https://www.youtube.com/watch?v=XSwFlzRd4oI",
            "https://www.youtube.com/watch?v=pJWQIIuoTWQ",
            "https://www.youtube.com/watch?v=MLPE9wdwPGk",
            "https://www.youtube.com/watch?v=M3R8pPX-v-Y",
            "https://www.youtube.com/watch?v=TkCg5s0ga1w",
            "https://www.youtube.com/watch?v=sO6dmYNB0rI",
            "https://www.youtube.com/watch?v=xTAU3pGr-9E",
            "https://www.youtube.com/watch?v=jBtmCZ4PcoI",
            "https://www.youtube.com/watch?v=8BAs7zAMdBo",
            "https://www.youtube.com/watch?v=wnRCeOeqxZo",
            "https://www.youtube.com/watch?v=vJWPHrgEl1g",
            "https://www.youtube.com/watch?v=_0gzB6y5D6I"

        };

        #endregion


        #region DownloadMethods


        private void VideoDownloader_DownloadProgressChanged(object sender, ProgressEventArgs e)
        {
            CustomVideoDownloader video = (CustomVideoDownloader)sender;
            var a = DownloadItems.FirstOrDefault(i => i.Id == video.DownloadItem.Id);
            a.DownloadedPercent = e.ProgressPercentage;
        }

        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        #endregion
    }
}
