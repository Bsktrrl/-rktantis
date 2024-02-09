using System.Text;

public class SpaceTextConverting : Singleton<SpaceTextConverting>
{
    public string SetText(string text)
    {
        StringBuilder builder = new StringBuilder();

        //DOn't modify the first letter
        builder.Append(text[0]);

        for (int i = 1; i < text.Length; i++)
        {
            //Check for upper-case letters
            if (char.IsUpper(text[i]))
            {
                //Insert a space-character before any upper-case letters
                builder.Append(' ');
            }

            //Then append the letter
            builder.Append(text[i]);
        }

        return builder.ToString();
    }
}
