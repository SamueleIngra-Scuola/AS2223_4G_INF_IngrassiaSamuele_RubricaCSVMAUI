
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
        public string Surname { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public Contact() { }

        public Contact(string surname, string name, string city)
        {
            this.Surname = surname;
            this.Name = name;
            this.City = city;
        }

        public Contact(string line)
        {
            string[] token = line.Split(',');

            Surname = token[0];
            Name = token[1];
            City = token[2];
        }

        //GetInfo Methods
        public string GetContactInfo()
        {
            return $"{Surname} | {Name} | {City}";
        }
    }

    private async void LoadCSVFile()
    {
        try
        {
            StreamReader file = new StreamReader(txtPath.Text);

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
            await DisplayAlert("Errore durante il caricamento del file", e.ToString(), "Ho capito");
        }
    }

    private void Search_Clicked(object sender, EventArgs e)
    {
        int n = 0;

        LoadCSVFile();

        string[] contactsList = new string[n_contacts];

        for (int i = 0; i < n_contacts; i++)
        {
            Contact contact = contacts[i];

            switch (cmbSurnameFilter.SelectedItem)
            {
                case "Contiene":
                    if (contact.Surname.ToUpper().Contains(txtSurname.Text.ToUpper()))
                    {
                        contactsList[n] = contact.GetContactInfo();
                        n++;
                    }
                    break;
                case "Inizia Con":
                    if (contact.Surname.ToUpper().StartsWith(txtSurname.Text.ToUpper()))
                    {
                        contactsList[n] = contact.GetContactInfo();
                        n++;
                    }
                    break;
                case "Termina Con":
                    if (contact.Surname.ToUpper().EndsWith(txtSurname.Text.ToUpper()))
                    {
                        contactsList[n] = contact.GetContactInfo();
                        n++;
                    }
                    break;
                default:
                    {
                        contactsList[n] = contact.GetContactInfo();
                        n++;
                    }
                    break;
            }
        }

        Array.Resize<string>(ref contactsList, n);
        
        lstContacts.ItemsSource = contactsList;
        lstContacts.Header = $"Rubrica ({n} Contatti)";    
}

    private async void Browse_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync();
        if (result != null)
            txtPath.Text = result.FullPath.ToString();
        else
            await DisplayAlert("Errore", "Non è stato inserito alcun file!", "Ho capito");
    }
}

