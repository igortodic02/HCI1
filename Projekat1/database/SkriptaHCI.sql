-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema HCI
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema HCI
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `HCI` DEFAULT CHARACTER SET utf8 ;
USE `HCI` ;

-- -----------------------------------------------------
-- Table `HCI`.`Korisnik`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `HCI`.`Korisnik` (
  `korisnickoIme` VARCHAR(50) NOT NULL,
  `lozinka` VARCHAR(100) NOT NULL,
  `tema` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`korisnickoIme`),
  UNIQUE INDEX `korisnickoIme_UNIQUE` (`korisnickoIme` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `HCI`.`Film`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `HCI`.`Film` (
  `naziv` VARCHAR(50) NOT NULL,
  `opis` VARCHAR(20000) NOT NULL,
  `zanr` VARCHAR(200) NOT NULL,
  `trajanje` INT NOT NULL,
  `3D` TINYINT(2) NOT NULL,
  `slika` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`naziv`),
  UNIQUE INDEX `Naziv_UNIQUE` (`naziv` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `HCI`.`Sala`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `HCI`.`Sala` (
  `broj` INT NOT NULL,
  `redovi` INT NOT NULL,
  `kolone` INT NOT NULL,
  PRIMARY KEY (`broj`),
  UNIQUE INDEX `broj_UNIQUE` (`broj` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `HCI`.`Termin`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `HCI`.`Termin` (
  `terminId` INT NOT NULL AUTO_INCREMENT,
  `nazivFilm` VARCHAR(50) NOT NULL,
  `brojSala` INT NOT NULL,
  `vrijeme` TIME NOT NULL,
  PRIMARY KEY (`terminId`),
  INDEX `fk_Film_has_Sala_Sala1_idx` (`brojSala` ASC) VISIBLE,
  INDEX `fk_Film_has_Sala_Film_idx` (`nazivFilm` ASC) VISIBLE,
  UNIQUE INDEX `terminId_UNIQUE` (`terminId` ASC) VISIBLE,
  CONSTRAINT `fk_Film_has_Sala_Film`
    FOREIGN KEY (`nazivFilm`)
    REFERENCES `HCI`.`Film` (`naziv`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Film_has_Sala_Sala1`
    FOREIGN KEY (`brojSala`)
    REFERENCES `HCI`.`Sala` (`broj`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `HCI`.`Kupovina`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `HCI`.`Kupovina` (
  `idKupovina` INT NOT NULL AUTO_INCREMENT,
  `sjediste` VARCHAR(3000) NOT NULL,
  `cijena` INT NOT NULL,
  `datum` DATE NOT NULL,
  `terminId` INT NOT NULL,
  `korisnickoIme` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`idKupovina`),
  UNIQUE INDEX `idKupovina_UNIQUE` (`idKupovina` ASC) VISIBLE,
  INDEX `fk_Kupovina_Termin1_idx` (`terminId` ASC) VISIBLE,
  INDEX `fk_Kupovina_Korisnik1_idx` (`korisnickoIme` ASC) VISIBLE,
  CONSTRAINT `fk_Kupovina_Termin1`
    FOREIGN KEY (`terminId`)
    REFERENCES `HCI`.`Termin` (`terminId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Kupovina_Korisnik1`
    FOREIGN KEY (`korisnickoIme`)
    REFERENCES `HCI`.`Korisnik` (`korisnickoIme`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `HCI`.`KupovinaDeleted`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `HCI`.`KupovinaDeleted` (
  `idKupovinaDeleted` INT NOT NULL AUTO_INCREMENT,
  `sjediste` VARCHAR(45) NOT NULL,
  `cijena` INT NOT NULL,
  `NazivFIlm` VARCHAR(50) NOT NULL,
  `vrijeme` TIME NOT NULL,
  `datum` DATE NOT NULL,
  `korisnickoIme` VARCHAR(50) NOT NULL,
  `brojSale` INT NOT NULL,
  PRIMARY KEY (`idKupovinaDeleted`),
  UNIQUE INDEX `idKupovinaDeleted_UNIQUE` (`idKupovinaDeleted` ASC) VISIBLE,
  INDEX `fk_KupovinaDeleted_Korisnik1_idx` (`korisnickoIme` ASC) VISIBLE,
  INDEX `fk_KupovinaDeleted_Sala1_idx` (`brojSale` ASC) VISIBLE,
  CONSTRAINT `fk_KupovinaDeleted_Korisnik1`
    FOREIGN KEY (`korisnickoIme`)
    REFERENCES `HCI`.`Korisnik` (`korisnickoIme`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_KupovinaDeleted_Sala1`
    FOREIGN KEY (`brojSale`)
    REFERENCES `HCI`.`Sala` (`broj`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

INSERT INTO `hci`.`korisnik` (`korisnickoIme`, `lozinka`, `tema`) VALUES ('admin', 'admin', 'dark');

INSERT INTO `hci`.`sala` (`broj`, `redovi`, `kolone`) VALUES ('1', '15', '20');
INSERT INTO `hci`.`sala` (`broj`, `redovi`, `kolone`) VALUES ('2', '15', '15');
INSERT INTO `hci`.`sala` (`broj`, `redovi`, `kolone`) VALUES ('3', '12', '16');

INSERT INTO `hci`.`film` (`naziv`, `opis`, `zanr`, `trajanje`, `3D`, `slika`) VALUES ('Interstellar', 'Interstellar je jedan od najambicioznijih i najemotivnijih science-fiction filmova 21. vijeka, režiran od strane Christophera Nolana, poznatog po filmovima koji kombinuju intelektualnu kompleksnost, vizuelnu spektakularnost i snažan emotivni naboj. Premijerno prikazan 2014. godine, film je odmah privukao pažnju kritike i publike zbog svoje hrabre kombinacije naučne vjerodostojnosti, filozofske dubine i porodične intime. Priča se odvija u bliskoj budućnosti, u svijetu koji se suočava s globalnom ekološkom katastrofom. Zemlja je iscrpljena, poljoprivredne kulture izumiru jedna za drugom, a pijesak neprestano nagriza civilizaciju. U takvom sumornom ambijentu, Interstellar otvara pitanje koje ljude prati od početka vremena: šta smo spremni žrtvovati da bismo preživjeli? I postoji li nešto što nadilazi čak i prostor i vrijeme—nešto što nas povezuje bez obzira na udaljenost?\n\nGlavni lik, Cooper (Matthew McConaughey), bivši pilot NASA-e i inženjer koji se pretvorio u farmera, predstavlja oličenje čovjeka rastrzanog između dužnosti prema svojoj djeci i potrebe da učini nešto veće za čovječanstvo. Njegov odnos s kćerkom Murph ključna je emocionalna nit filma. Njihova veza je snažna, ali istovremeno bolna, obojena osjećajem izdaje kada Cooper prihvati misiju da napusti Zemlju i priključi se tajnom NASA-inom projektu koji planira kolonizaciju udaljenih planeta preko misterioznog crvotočina otkrivenog blizu Saturna.\n\nFilm se ističe po načinu na koji spaja nauku i emocije. Nolan i njegov tim sarađivali su s fizičarem Kipom Thorneom kako bi osigurali naučnu preciznost koncepta crvotočina, relativiteta i singularnosti. Zahvaljujući tome, scena prolaska kroz crvotočinu i vizuelni prikaz Gargantue, supermasivne crne rupe, postali su ne samo spektakularni već i naučno obrazovni momenti u kinematografiji. Čak je i sam render Gargantue bio toliko detaljan da je izračuni korišteni za specijalne efekte poslužili za naučne publikacije.\n\nJedna od najvećih tema filma jeste relativnost vremena. Kada Cooper i tim slete na planetu gdje vrijeme teče mnogo sporije zbog blizine crne rupe, nekoliko njihovih sati pretvara se u decenije na Zemlji. Ova ideja ne funkcioniše samo kao naučni koncept, već kao brutalno emotivni udarac—Cooper gleda kako mu djeca odrastaju i stare kroz snimljene poruke, dok je on s druge strane svemira zakovan u trenutku. U tim scenama film budi osjećaj nemoći i melankolije, podsjećajući nas da je vrijeme najokrutnija sila u univerzumu. McConaugheyjeva gluma posebno briljira u tim trenucima, noseći na sebi sav teret roditelja koji zna da je možda zauvijek propustio živote svoje djece.\n\nS druge strane, Murphin razvojni put pokazuje drugu polovinu emocionalnog kruga. Kao briljantna naučnica, ona provodi godine pokušavajući riješiti gravitacionu jednačinu koja bi omogućila ljudima da napuste Zemlju. Iako ljuta i slomljena zbog očeve odluke, njena upornost i briljantnost postaju ključ za spas čovječanstva. Simbolika njihovog odnosa dodatno se produbljuje motivom sata koji Cooper ostavlja Murph. Sat postaje most između dimenzija, svojevrsna komunikaciona nit koja povezuje oca i kćer, ali i simbol nade da vrijeme nije prepreka kada je veza dovoljno snažna.\n\nVrhunac filma odvija se unutar tzv. teserakta—višedimenzionalnog prostora gdje Cooper prolazi kroz granični prostor između fizike i metafizike. Ovaj segment je među najkontroverznijima u filmu, jer prelazi granicu klasične nauke i ulazi u domen teorijske fizike i filozofskih interpretacija. Međutim, teserakt je predstavljen kao logičan kraj Nolanovog istraživanja odnosa čovjeka s univerzumom: ako ljubav, sjećanje i namjera mogu imati trajnu formu, onda možda mogu imati i fizički odraz u višoj dimenziji postojanja. Cooper tu shvata da je upravo on bio „duh“ koji je komunicirao s Murph u njenom djetinjstvu, zatvarajući narativni krug koji film gradi od samog početka.\n\nHans Zimmerova muzika igra ogromnu ulogu u stvaranju atmosfere. Koristeći orgulje, Zimmer stvara dojam grandioznosti, duhovnosti i hladne beskonačnosti svemira. Soundtrack nosi film, često preuzimajući primat nad dijalogom i postajući glas emocija koje likovi ne izgovaraju. Posebno se ističe tema “Stay”, koja savršeno encapsulira nostalgiju, bol i ljubav koji prožimaju svaku Cooperovu odluku.', 'Avantura, Drama, Naučna fantastika', '166', '1', '../../../assets/movieImages/Interstellar.jpg');
INSERT INTO `hci`.`film` (`naziv`, `opis`, `zanr`, `trajanje`, `3D`, `slika`) VALUES ('Memento', 'Memento je intenzivan psihološki triler Christophera Nolana koji se ističe jedinstvenom narativnom strukturom i napetom atmosferom. Film prati Leonarda Shelbyja, čovjeka koji pati od rijetkog oblika kratkotrajnog gubitka pamćenja, zbog čega ne može zadržati nova sjećanja duže od nekoliko minuta. Iako zarobljen u vlastitom mentalnom haosu, Leonard je odlučan da pronađe odgovore koji mu izmiču, oslanjajući se isključivo na bilješke, fotografije i tetovaže kao jedine pouzdane tragove.Memento gledaoce uvlači u neobičnu perspektivu čovjeka koji svakog trenutka može izgubiti nit onoga što se događa. Radnja je oblikovana tako da podstiče stalnu napetost, neizvjesnost i analizu, dok atmosfera ostaje mračna, tajanstvena i emocionalno nabijena. Film se poigrava percepcijom, pamćenjem i identitetom, postavljajući pitanje koliko zapravo možemo vjerovati vlastitom umu. Nolan vješto gradi dinamiku koja tjera gledaoca da bude aktivni učesnik, a ne samo posmatrač. Memento je film koji se ne gleda pasivno — on se doživljava.', 'Avantura, Drama, Misterija', '110', '1', '../../../assets/movieImages/Memento.png');
INSERT INTO `hci`.`film` (`naziv`, `opis`, `zanr`, `trajanje`, `3D`, `slika`) VALUES ('Gospodar prstenova', 'Gospodar prstenova (The Lord of the Rings) jedan je od najvažnijih i najuticajnijih fantasy serijala ikada napisanih. Nastao iz pera J.R.R. Tolkiena, ovaj epski svijet predstavlja spoj mitologije, lingvistike, herojske drame i duboke simbolike. Radnja prati skromnog hobita Froda Baginsa, kojem je povjerena gotovo nemoguća misija – uništiti Jedinstveni Prsten, izvor moći mračnog gospodara Saurona. Tolkienov svijet obiluje bogatom istorijom, različitim rasama, jezicima i kulturama, stvarajući osjećaj da Strednja Zemlja zaista postoji negdje izvan granica mašte. Priča ne prikazuje dobro i zlo samo kao jednostavne suprotnosti, već istražuje hrabrost, žrtvu, iskušenje i istrajnost u trenucima najmračnijih prepreka. Filmska trilogija Petera Jacksona dodatno je učvrstila status ovog djela kao modernog mita. Uz spektakularne vizuelne efekte, emotivnu glumu i vjernu atmosferu, filmovi su približili Tolkienov svijet globalnoj publici. Gospodar prstenova ostaje bezvremenska priča o prijateljstvu, odgovornosti i borbi protiv tame, podsjećajući nas da i „najmanji među nama“ mogu promijeniti tok svijeta.', 'Avantura, Drama, Fantazija, Porodični', '256', '1', '../../../assets/movieImages/Gospodar prstenova.png');
INSERT INTO `hci`.`film` (`naziv`, `opis`, `zanr`, `trajanje`, `3D`, `slika`) VALUES ('Shutter island', 'Shutter Island je psihološki triler Martina Scorsesea koji se ističe mračnom atmosferom, napetošću i slojevitom narativnom strukturom. Radnja prati dvojicu američkih maršala, Teddyja Danielsa i Chucka Aulea, koji stižu na izolovano ostrvo Shutter Island kako bi istražili nestanak pacijentkinje iz psihijatrijske ustanove za kriminalno oboljele. Već od prvih trenutaka, ostrvo djeluje neprijatno, gotovo klaustrofobično, a svakim korakom istraga postaje sve zamršenija. Film se igra percepcijom, sumnjom i mentalnim stanjima, stvarajući osjećaj stalne nesigurnosti. Teddy, koga glumi Leonardo DiCaprio, suočava se ne samo s neobičnim ponašanjem osoblja i pacijenata, već i s vlastitim traumama koje se počinju miješati s realnošću istrage. Scorsese vješto gradi tenziju kroz simboliku, vizije i neočekivane detalje, čineći da gledalac nikada nije potpuno siguran šta je stvarno, a šta plod uma. Mračna estetika, upečatljiva muzika i atmosferični kadrovi dodatno pojačavaju osjećaj izolacije i prijetnje. Shutter Island nije samo triler, već duboko psihološko putovanje koje istražuje granice ljudske psihe, identiteta i istine. Film ostaje upamćen kao jedno od najintenzivnijih i najintrigantnijih Scorsesejevih djela.', 'Akcija, Drama, Krimi, Misterija, Triler', '105', '0', '../../../assets/movieImages/Shutter island.jpg');
INSERT INTO `hci`.`film` (`naziv`, `opis`, `zanr`, `trajanje`, `3D`, `slika`) VALUES ('Amadeus', 'Amadeus je raskošna biografsko-dramska interpretacija života Wolfganga Amadeusa Mozarta, režirana od strane Miloša Formana. Iako nije klasična biografija, film kroz priču Antonija Salierija stvara fascinantnu kombinaciju istorije, drame i ljubomore. Prikazan kao čovjek opsjednut Mozartovim genijem, Salieri postaje vodič kroz svijet bečke muzike 18. vijeka, gdje se briljantnost i ludost često dodiruju.Film se ističe nevjerovatnim vizuelnim stilom, raskošnim kostimima i snažnim glumačkim izvedbama, posebno Toma Hulcea i F. Murraya Abrahama. Mozart je prikazan kao talentovani, ali impulzivni umjetnik čiji dar nadilazi razumijevanje ljudi oko njega. Amadeus istražuje teme zavisti, božanskog nadahnuća i cijene koju umjetnik plaća za genijalnost. Uz predivnu muziku koja oblikuje emocije i ritam radnje, film ostaje jedno od najimpresivnijih kinematografskih djela o kreativnosti i ljudskoj slabosti.', 'Biografija, Drama, Istorijski, Mjuzikl', '189', '0', '../../../assets/movieImages/Amadeus.png');
INSERT INTO `hci`.`film` (`naziv`, `opis`, `zanr`, `trajanje`, `3D`, `slika`) VALUES ('Predestination', 'Predestination je intrigantni sci-fi triler koji prati tajanstvenog agenta na njegovoj najvažnijoj misiji: spriječiti veliki zločin putujući kroz vrijeme. Film stvara intenzivnu atmosferu ispunjenu misterijom, razgovorima koji postepeno otkrivaju duboke teme i neočekivane obrate. Uz snažne glumačke nastupe i pametno konstruiranu priču, Predestination nudi iskustvo koje drži pažnju do posljednjeg trenutka.', 'Drama, Misterija, Naučna fantastika', '96', '0', '../../../assets/movieImages/Predestination.png');
INSERT INTO `hci`.`film` (`naziv`, `opis`, `zanr`, `trajanje`, `3D`, `slika`) VALUES ('12 angry men', '12 Angry Men je jedan od najznačajnijih filmova klasične američke kinematografije, režiran 1957. godine od strane Sidneya Lumeta. Iako se gotovo cijeli film odvija u jednoj prostoriji, njegova snaga leži u briljantnoj dinamici među likovima, napetosti koja postepeno raste i sposobnosti da iz minimalnog vizuelnog okvira izvuče maksimalan emotivni i intelektualni utisak. Radnja prati porotu od dvanaest muškaraca kojima je povjerena odluka o sudbini mladića optuženog za ubistvo. Iako se čini da je slučaj jednostavan, uskoro postaje jasno da ništa nije tako jednostavno kada se pogledi, uvjerenja i predrasude počnu sudarati.Film se izdvaja time što kroz razgovor — ponekad smiren, ponekad žestok — razotkriva različite ljudske karaktere i njihove unutrašnje motive. Svaki član porote donosi u sobu vlastito životno iskustvo, strahove i uvjerenja, a upravo kroz to počinjemo shvatati kako subjektivnost može uticati na nešto što bi trebalo biti potpuno racionalno i objektivno. Napetost raste polako, ali sigurno, dok se atmosfera zagrijava i prostor sve više djeluje klaustrofobično. Lumet koristi ovo ograničenje da pokaže kako se sukobi i tenzije gomilaju, stvarajući gotovo pozorišni ugođaj u kojem svaka riječ ima težinu. 12 Angry Men nije samo film o poroti i sudu — to je film o ljudskoj prirodi, pristrasnosti, odgovornosti i hrabrosti da se zauzme stav, čak i kada je to teško i nepopularno. Poruka filma je snažna i univerzalna: odluke koje donosimo, posebno one koje utiču na tuđe živote, zahtijevaju promišljenost, empatiju i spremnost da saslušamo druge. Upravo zato film ostaje relevantan i danas, decenijama nakon premijere. Uz izvanredne glumačke izvedbe i inteligentan, oštar dijalog, 12 Angry Men predstavlja primjer kako se vrhunska drama može izgraditi iz čiste snage argumentacije i međuljudske interakcije. To je film koji gledaoce podstiče na razmišljanje, preispitivanje i razumijevanje — i zato se smatra remek-djelom koje ostavlja dugotrajan utisak.', 'Drama, Krimi, Misterija', '88', '0', '../../../assets/movieImages/12 angry men.png');
INSERT INTO `hci`.`film` (`naziv`, `opis`, `zanr`, `trajanje`, `3D`, `slika`) VALUES ('Tenet', 'Tenet je ambiciozni akcijsko-science fiction film Christophera Nolana koji se poigrava konceptom vremena na jedinstven način. Radnja prati tajnog agenta u misiji koja nadilazi klasične špijunske okvire, jer se suočava s tehnologijom koja može „invertovati“ vrijeme. Film kombinuje spektakularne akcione sekvence, kompleksnu naraciju i misterioznu atmosferu, stvarajući intenzivno iskustvo koje izaziva gledaoce da aktivno prate svaki detalj.', 'Akcija, Drama, Naučna fantastika', '150', '1', '../../../assets/movieImages/Tenet.jpeg');

INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('12 angry men', '1', '20:00:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('12 angry men', '3', '22:15:00');

INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Tenet', '2', '20:00:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Tenet', '2', '21:00:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Tenet', '2', '22:00:00');

INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Predestination', '1', '12:30:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Predestination', '1', '18:30:00');

INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Amadeus', '1', '20:30:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Amadeus', '3', '20:45:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Amadeus', '1', '21:30:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Amadeus', '1', '22:40:00');

INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Shutter island', '1', '12:40:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Shutter island', '1', '22:40:00');

INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Gospodar prstenova', '1', '12:00:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Gospodar prstenova', '1', '15:00:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Gospodar prstenova', '2', '19:00:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Gospodar prstenova', '3', '21:00:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Gospodar prstenova', '3', '23:00:00');

INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Memento', '1', '22:00:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Memento', '1', '19:45:00');

INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Interstellar', '2', '22:40:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Interstellar', '3', '21:10:00');
INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES ('Interstellar', '3', '23:30:00');

