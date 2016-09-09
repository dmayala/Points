using Android.OS;
using Java.Interop;
using Newtonsoft.Json;

namespace Points.Droid.Utils
{
    public static class ParcelableUtil
    {
        public static IParcelable Wrap<T>(T sessions)
        {
            var json = JsonConvert.SerializeObject(sessions);
            return new ParcelableString(json);
        }

        public static T Unwrap<T>(IParcelable parcelable)
        {
            var parcelStr = parcelable as ParcelableString;
            return JsonConvert.DeserializeObject<T>(parcelStr.Text);
        }
    }

    public class ParcelableString : Java.Lang.Object, IParcelable
    {
        public string Text { get; internal set; }

        public ParcelableString(string text)
        {
            Text = text;
        }

        public int DescribeContents()
        {
            return 0;
        }

        public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
        {
            dest.WriteString(this.Text);
        }

        [ExportField("CREATOR")]
        public static IParcelableCreator GetCreator()
        {
            return new ParcelableCreator();
        }
    }

    public class ParcelableCreator : Java.Lang.Object, IParcelableCreator
    {
        Java.Lang.Object IParcelableCreator.CreateFromParcel(Parcel source)
        {
            return new ParcelableString(source.ReadString());
        }

        Java.Lang.Object[] IParcelableCreator.NewArray(int size)
        {
            return new ParcelableString[size];
        }
    }
}