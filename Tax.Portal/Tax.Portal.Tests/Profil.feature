Jellemző: Felhasználói profil
	Ahhoz, hogy használni tudjam a Kontakt Portált, mint Ügyfél, tudnom kell
	regisztrálni,
	bejelentkezni és kijelentkezni,
	adataimat módosítani.

Forgatókönyv vázlat: Regisztráció
	Adott a regisztrációs oldal
	Ha megadom a <felhasználó név> <Email cím> <jelszó> és <jelszó mégegyszer> regisztrációs adatot
	Akkor a rendszer tájékoztat a regisztráció <eredmény>-éről
Példák: 
	| felhasználó név | Email cím | jelszó | jelszó mégegyszer | eredmény |
	| 'tesztfelhasznalo' | 'teszt@egroup.hu' | 'pas1234PAS' | 'pas1234PAS' | 'sikeres' |
#	| 'tesztfelhasználó' | '@egroup.h'u | 'pas1234PAS' | 'pas1234PAS' | 'sikertelen' |
#	| 'tesztfelhasználó' | 'teszt@egroup.hu' | 'PAS' | 'PAS' | 'sikertelen' |
#	| 'tesztfelhasználó' | 'teszt@egroup.hu' | 'pas1234PAS' | 'pas1234PA' | 'sikertelen' |	

Forgatókönyv vázlat: Bejelentkezés
	Adott a bejelentkezési oldal
	Ha megadom a <felhasználó név> és <jelszó> bejelentkezési adatot
	Akkor a rendszer tájékoztat a bejelentkezés <eredmény>-éről
Példák: 
	| felhasználó név | jelszó | eredmény |
	| 'tesztfelhasznalo' | 'pas1234PAS' | 'sikeres' |
#	| 'tesztfelhasználó' | - | 'sikertelen' |

Forgatókönyv: Kijelentkezés
	Adott a bejelentkezett felhasználó
	Ha kijelentkezem
	Akkor a rendszer tájékoztat a kijelentkezésről

Forgatókönyv vázlat: Jelszóvisszaállítás kezdeményezése
	Adott a jelszóvisszaállítási oldal
	Ha megadom a <email cím>-et
	És kezdeményezem a jelszó visszaállítást
	Akkor a rendszer tájékoztat a jelszóvisszaállítás teendőiről
Példák:
	| email cím         |
	| 'teszt@egroup.hu' |
#	| '@egroup.hu' |

Forgatókönyv vázlat: Jelszóvisszaállítás végrehajtása
	Adott a jelszóvisszaállítási <token>
	Ha megadom a <jelszó1> és <jelszó2> jelszó adatot 
	És megerősítem a jelszó visszaállítást
	Akkor a rendszer tájékoztat a jelszóvisszaállítás <eredmény>-ről
Példák:
	| token | jelszó1      | jelszó2      | eredmény  |
	| '123' | 'pas1234PAS' | 'pas1234PAS' | 'sikeres' |
#	| '123' | 'pas1234PAS' | '1234PAS' | 'sikertelen' |

Forgatókönyv vázlat: Emailcím változtatás kezdeményezése
	Adott a bejelentkezett felhasználó az emailcím változtatása oldalon
	Ha megadom az <email cím> email címet
	És kezdeményzem az email cím megváltoztatását
	Akkor a rendszer tájékoztat az email cím változtatás teendőiről
Példák:
	| email cím         |
	| 'teszt@egroup.hu' |
#	| '@egroup.hu' |

Forgatókönyv vázlat: Emailcím változtatás végrehajtása
	Adott az email cím változtatási <token>
	Ha megerősítem az email cím változtatást
	Akkor a rendszer tájékoztat az email cím változtatás <eredmény>-ről
Példák:
	| token | eredmény  |
	| '123' | 'sikeres' |
#	| '123' | 'sikertelen' |

Forgatókönyv vázlat: Egyéb profiladatok változtatása
	Adott a bejelentkezett felhasználó a profiladatok változtatása oldalon
	Ha megadom a <Sinosz-tagság>, <Tagsági azonosító>, <Születési dátum>, <Kommunikációs igény> és <Eszközigény> adatot
	Akkor a rendszer tájékoztat az adatváltoztatás <eredmény>-éről 
Példák:
	| Sinosz-tagság | Tagsági azonosító | Születési dátum | Kommunikációs igény | Eszközigény | eredmény |
	| 'igen'        | '12345'           | '1990.01.01'    | 'igen'              |  'igen'     | 'sikeres'|

