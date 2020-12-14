# XamarinChatClient 2

Tässä tehtävässä jatkat edellistä chat-sovellustasi, eli käytä pohjana aiemmin tekemääsi tuotosta tehtävästä: https://classroom.github.com/a/5Q_ki_z0

Osoitteessa http://13.74.41.52:8080/ pyörii yksinkertainen viestinvälitys-palvelin. Voit testata sen toimintaa avaamalla http://13.74.41.52:8080/hello_world osoitteen selaimessasi. Jos saat selaimeesi vastauksen palvelusta, on se myös käynnissä.

Edellä mainittu **hello_world**-komento käyttää **GET**-tyyppistä pyyntöä palvelimelle, eli perus www-selaimella saa sen tiedot luettua.

Muut tarjolla olevat toiminnot ovat **POST**-tyyppisiä pyyntöjä ja niissä taustajärjestelmä odottaa saavansa parametrejä. Pyynnöt ovat:
- **/register**
- **/login**
- **/sendmessage**
- **/getmessages**

**http://13.74.41.52:8080/register** -pyyntö odottaa saavansa 2 parametria, jotka ovat **username** ja **password**. Jos rekisteröinti onnistuu, palvelu palauttaa **Rekisteröinti onnistui**. Jos se epäonnistuu niin palvelu palauttaa **Rekisteröinti epäonnistui: <erillinen virheviesti>**.
```
{ "username": "pena", "password":"penansalasana"}
```
Jos edellinen operaatio onnistuu, palvelin palauttaa jotain tämänkaltaista:
```
{ "result": "Rekisteröinti onnistui"}
```
Jos taas operaatio ei toimi, esim. olemassa olevan käyttäjätunnuksen takia niin se palauttaa jotain tällaista:
```
{ "result": "Fail: Rekisteröinti epäonnistui: käyttäjänimi on jo käytössä"}
```

**http://13.74.41.52:8080/login** -pyyntö odottaa saavansa myös 2 parametria, jotka ovat **username** ja **password**. Nämä 2 pitää kuitenkin olla jo rekisteröity aiemmin, joten jos rekisteröinti puuttuu ei kirjautuminen vielä onnistu. Jos kirjautuminen onnistuu, palauttaa palvelin **kirjautumisavaimen** joka sinun tulee ottaa talteen sovelluksessasi seuraavia pyyntöjä varten. Kolmas parametri jonka palvelimelle voi lähettää on **chatnick**. Tätä käytetään käyttäjän omana nimimerkkinä chat-näkymässä.
```
{ "username": "pena", "password":"penansalasana", "chatnick": "PenanChatNikki"}
```
Jos edellinen operaatio onnistuu, palvelin palauttaa jotain tämänkaltaista:
```
{ "accesskey": "123123123123132", "result":"Onnistui"}
```
Jos taas operaatio ei toimi, esim. puuttuvan käyttäjätunnuksen takia niin se palauttaa jotain tällaista:
```
{"accesskey": "", "result": "Fail: käyttäjää ei löydy"}
```


**http://13.74.41.52:8080/sendmessage** -pyyntö tarvitsee 2 parametria. Ensimmäinen on **accesskey**, jonka sait **login**-komennolla parametrina. Toinen on oma viestisi **message**, joka sisältää itse lähettämäsi viestin. Jos accesskey on ok ja viestin lähetys onnistuu, palvelin palauttaa **Viesti lähetetty palvelimelle**. Jos se taas epäonnistuu, palauttaa palvelin esim. **Epäkelpo kirjautumisavain**.
```
{ "accesskey": "123123123123132", "message":"ykäsonni täs terve"}
```
Jos viestin lähettäminen onnistuu, palvelin palauttaa jotain tämänkaltaista:
```
{ "result": "Viesti lähetetty palvelimelle" }
```
Jos taas operaatio ei toimi, esim. sen takia ettei käyttäjä ole vielä kirjautunut ja accesskey on väärä:
```
{ "result": "Fail: Epäkelpo kirjautumisavain"}
```


**http://13.74.41.52:8080/getmessages** -pyynnöllä on vain 1 parametri, joka on aiemmin saamasi **accesskey**. Paluuarvona palvelin palauttaa **result**-tekstin lisäksi listan joka sisältää kaikki palveluun lähetetyt viestit kaikilta käyttäjiltä heidän omilla nimimerkeillään. Paluulista on siis esim. tällainen:
```
{"result":"Onnistui", "chatmessages":
	[
		{ "nick": "pena", "message": "morooooo" },
		{ "nick": "simo", "message": "tsau" },
		{ "nick": "veikko", "message": "kkona" },
		{ "nick": "ismo", "message": "tolokun poika" },
		{ "nick": "torsti", "message": "ON" }
	]
}
```
Jos komennon suoritus epäonnistuu, palautuu palvelimelta jälleen tämän tyyppinen viesti:
```
{"result":"Fail: Epäkelpo kirjautumisavain", "chatmessages": []}
```

Voit testata em. viestejä myös **PostMan**-työkalulla. Alla esimerkki **sendmessage**-viestin sisällöstä.

![Postman](/Images/1.PNG?raw=true)

Kun olet sisäistänyt ylläolevan ohjeistuksen ja saanut testattua jokaisen komennon **Postmanilla**, voit siirtyä toteuttamaan toimintoja omaan **XamarinChatClient**-sovellukseesi.

Lisää ensin koodiisi uusi **Shared.cs**-luokka. Lisää sinne ao. koodit jotta voit hyödyntää niitä kutsujen lähettämisessä. Joudut myös lataamaan projektiisi **Nuget**-paketin **Refit**.

```
using Refit;  
using System.Collections.Generic;  
using System.Threading.Tasks;

namespace AwesomeApp
{
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class UserAccess
    {
        public string accesskey { get; set; }
        public string result { get; set; }
    }

    public class ChatMessageSend
    {
        public string accesskey { get; set; }
        public string message { get; set; }
    }

    public class ChatMessageReceive
    {
        public string nick { get; set; }
        public string message { get; set; }
    }

    public class ChatMessages
    {
        public List<ChatMessageReceive> chatmessages { get; set; }
        public string result { get; set; }
    }

    public interface IAuthAPI
    {
        [Post("/login")]
        Task<UserAccess> Login([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> data);

        [Post("/register")]
        Task<string> Register([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> data);

        [Post("/sendmessage")]
        Task<string> SendMessage([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> data);

        [Post("/getmessages")]
        Task<ChatMessages> GetMessages([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> data);
    }
}
```

Kun olet saanut edellä listatut koodit lisättyä omaan projektiisi ja korjattua mahdolliset error-viestit niin voit sen jälkeen lisätä omaan koodiisi **register**-napin painalluksesta toiminnon rekisteröinnille. Alla koodiesimerkki miten sen voi toteuttaa. Huomaa kuitenkin että joudut muokkaamaan siihen oikeat kenttien nimet esim. **nick** ja **password**-kenttien tilalle.
```
if (!string.IsNullOrEmpty(nick.Text) && !string.IsNullOrEmpty(password.Text))
{
	var authAPI = RestService.For<IAuthAPI>("http://13.74.41.52:8080");
	Dictionary<string, string> data = new Dictionary<string, string>();
	data.Add("username", nick.Text);
	data.Add("password", password.Text);
	SimpleServerResponse res = await authAPI.Register(data);
	infoLabel.Text = res.result.ToString();
	//infoLabel.Text = nick.Text + " has been registered";
}
```

Kun olet saanut rekistöinnin toimimaan, voit toteuttaa loput ominaisuudet sovellukseesi valmiiksi. Sovelluksen pitää siis toteuttaa seuraavat:
* Register
* Login
* SendMessage
* GetMessages

Jälkimmäistä voi aluksi suorittaa aina sen jälkeen kun SendMessagea kutsutaan, mutta mieti myös sellaista vaihtoehtoa että tekisitkin sitä esim. 10 sekunnin välein automaattisesti.

Käytännössä siis sovelluksen pitäisi toimia täsmälleen kuten minkä tahansa muunkin chat-sovelluksen. Chattinäkymän sisältö pitäisi siis olla jotain sen kaltaista kuin ao. kuvankaappauksessa:

![Chat](/Images/2.PNG?raw=true)

Kun chat-sovelluksesi toimii ja näet omien viestiesi lisäksi myös luokkatovereiden viestit kännykkäsi ruudulla, palauta koodit tähän repon git commit ja push komennoilla.
