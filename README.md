# HCI1

# Upustvo za pokretanje aplikacije

Na putanji Projekat1/database/SkriptaHCI.sql nalazi se skripta za kreiranje baze podataka sa testnim podacima.Testni podaci uključuju kreiran korisnički nalog za administratora sa korisničkim imenom "admin" i lozinkom "admin", kao i 8 kreiranih filmova i po nekoliko termina za svaki od filmova.

U fajlu Projekat1/Projekat1/bin/Debug/net7.0-windows/appsettings.json se nalazi konekcioni string za bazu podataka:

{
  "ConnectionStrings": {
    "db": "Server=127.0.0.1;Port=3306;Database=HCI;UserId = root; Password = root;"
  }
}

Potrebno je skriptu za kreiranje baze podataka izvršiti na odgovarajućoj konekciji.

Aplikacija se može pokrenuti na jedan od sljedećih načina:
1)Pokretanjem pomoću prečice KinoAppBL
2)Pokretanjem pomoću izvršnog fajla Projekat1/Projekat1/bin/Debug/net7.0-windows/Projekat1.exe
3)Otvaranjem riješenja Projekat1/Projekat1.sln u Visual Studiu, i izvršavanjem koda


# Upustvo za korišćenje aplikacije

Prvobitno se vrši prijava ( na postojeći nalog) ili registracija ( kreiranje novog korisničkog naloga). Na istoj stranici se bira i jezik aplikacije. Nakon prijave/registracije prikazuje se početna stranica na kojoj su prikazani svi dostupni filmovi. Filmovi se mogu pregledati, sortirati i filtrirati. Na istoj stranici pri vrhu se nudi opcija za promjenu teme aplikacije. U gornjem desnom uglu se nalazi dugme za odjavu sa sistema.

Ukoliko je admin prijavljen na sistem u navbar-u posjeduje opciju za kreiranjem novog filma. Pri pregledanju filma moze da odabere opcije da ažurira ili da obriše film koji trenutno pregleda.

Ukoliko je korisnik prijavljen na sistem u navbar-u posjeduje opciju za pregledanjem informacija vezanih za transakcije koje je izvršio. Prilikom pregledanja filma korisnik ima mogućnost kupovine karte za odabrani film. Kupovina se vrši biranjem termina i datuma kada se vrši projekcija filma. Nakon toga se prikazuju i biraju dostupna sjedišta u sali. Informacije vezane za transakciju se čuvaju.
