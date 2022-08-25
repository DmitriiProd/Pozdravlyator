using Amazon.EC2.Import;
using Hl7.Fhir.ElementModel.Types;
using java.io;
using Lucene.Net.Support;
using Microsoft.PowerBI.Api.Models;
using MvvmCross.Platforms.Uap.Binding;
using NHibernate.Mapping.ByCode;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using String = System.String;
using Type = System.Type;

namespace Pozdravlyator {
 class MainClass
{
    static HashMap<string, Long> birthdays = new HashMap<>();

    static final readonly int maxCheckDays = 7;

    public static void main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Укажите путь к файлу!");
            return;
        }

            File file = new File(args[0]);

        if (!file.exists())
        {
                Console.WriteLine("Указанный файл не найден! (" + file.getAbsolutePath() + ")");
            return;
        }

            Calendar curr = Calendar.getInstance();
        curr.setTimeInMillis(System.currentTimeMillis());
        curr.set(Calendar.HOUR_OF_DAY, 0);
        curr.set(Calendar.MINUTE, 0);
        curr.set(Calendar.SECOND, 0);
        curr.set(Calendar.MILLISECOND, 0);

        try
        {
            BufferedReader reader = new BufferedReader(new FileReader(file));
            string line = "";

            while ((line = reader.readLine()) != null)
            {
                string birthday = line.Split("\\|")[0];
                string name = line.Split("\\|")[1];

                SimpleDateFormat dateFormat = new SimpleDateFormat("dd.MM.yyyy");
                Date date = dateFormat.parse(birthday);

                Calendar calendar = Calendar.getInstance();
                calendar.setTime(date);

                checkBirthDay(curr, date, name);
            }
            birthdays = sortByValue(birthdays);

            birthdays.forEach((name, days)-> {
                if (days == 0) System.out.println("Сегодня день рождения у " + name);
                else System.out.println("У " + name + " день рождения через " + days + " " + formatName(days));
            });

        }
        catch (Exception ex)
        {
            ex.printStackTrace();
        }
    }

    static void checkBirthDay(Calendar current, Date date, string name)
    {
        Calendar calendar = Calendar.getInstance();
        calendar.setTime(date);

        if (current.equals(calendar))
        {
            birthdays.put(name, 0L);
        }

        if (current.before(calendar))
        {
            long remainingDays = TimeUnit.MILLISECONDS.toDays(
                    Math.abs(calendar.getTimeInMillis() - current.getTimeInMillis()));

            if (remainingDays <= maxCheckDays)
            {
                birthdays.put(name, remainingDays);
            }
        }
    }

    static HashMap<string, Long> sortByValue(HashMap<string, Long> hm)
    {

        return hm.entrySet()
        .stream()
        .sorted(Map.Entry.comparingByValue())
        .collect(Collectors.toMap(
                Map.Entry::getKey,
                Map.Entry::getValue,
                (e1, e2)->e1, LinkedHashMap::new));
    }

    static string formatName(long input)
    {

        final String[] names = new String[] { "день", "дня", "дней" };


        int cef;

        if (input % 10 == 1 && input % 100 != 11) cef = 0;

        else if (2 <= input % 10 && input % 10 <= 4 && (input % 100 < 10 || input % 100 >= 20)) cef = 1;

        else cef = 2;

        return names[cef];
    }

}
