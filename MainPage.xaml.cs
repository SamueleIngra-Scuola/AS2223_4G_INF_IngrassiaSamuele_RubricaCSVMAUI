
namespace RubricaCSVMAUI;

public partial class MainPage : ContentPage
{
    const int N_MAX_CONTACTS = 100;
    Contact[] contacts;
    int n_contacts;

    public MainPage()
    {
        InitializeComponent();
        contacts = new Contact[N_MAX_CONTACTS];
        n_contacts = 0;
    }

    public class Contact
    {
        string surname;
        string name;
        string city;

        public Contact() { }

        public Contact(string surname, string name, string city)
        {
            this.surname = surname;
            this.name = name;
            this.city = city;
        }

        public Contact(string line)
        {
            string[] token = line.Split(',');

            surname = token[0];
            name = token[1];
            city = token[2];
        }

        //GetInfo Methods
        public string GetContactInfo()
        {
            return $"{surname} | {name} | {city}";
        }

        public string GetSurname()
        {
            return surname;
        }

        public string GetName()
        {
            return name;
        }

        public string GetCity()
        {
            return city;
        }
    }

    private void LoadCSVFile()
    {
        try
        {
            StreamReader file = new StreamReader(CSVPathEnt.Text);

            string line = file.ReadLine();

            n_contacts = 0;

            while (line != null)
            {
                contacts[n_contacts] = new Contact(line);

                line = file.ReadLine();

                n_contacts++;
            }

            file.Close();
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.ToString(), "Errore durante il caricamento del file");
        }
    }

    private void SearchClicked(object sender, EventArgs e)
    {
        LoadCSVFile();
        string[] contactsList = new string[n_contacts];

        for (int i = 0; i < n_contacts; i++)
        {
            Contact contact = contacts[i];

            switch (SurnamePicker.SelectedItem)
            {
                case "Contiene":
                    if (contact.GetSurname().ToUpper().Contains(SurnameTxt.Text.ToUpper()))
                        contactsList[i] = contact.GetContactInfo();
                    break;
                case "Inizia Con":
                    if (contact.GetSurname().ToUpper().StartsWith(SurnameTxt.Text.ToUpper()))
                        contactsList[i] = contact.GetContactInfo();
                    break;
                case "Termina Con":
                    if (contact.GetSurname().ToUpper().EndsWith(SurnameTxt.Text.ToUpper()))
                        contactsList[i] = contact.GetContactInfo();
                    break;
                default:
                    contactsList[i] = contact.GetContactInfo();
                    break;
            }
        }

        ListView.ItemsSource = contactsList;
    }

    private async void BrowseClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync();
        CSVPathEnt.Text = result.FullPath.ToString();
    }
}

