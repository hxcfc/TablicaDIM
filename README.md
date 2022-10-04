
# Tablica DIM
Aplikacja została stworzona by wspomóc zarządzanie całym działem utrzymania ruchu 
składającym się z obszarów.

Obszar składa się z:
- pracowników,
- stanowisk,
- akcji zaplanowanych na dany termin,
- planera pracy w weekend,
- planera zmian pracowników,
- planera urlopów pracowników.
## Demo aplikacji
Przegląd działania aplikacji. Czarne obrysy wyskakujących okien wygenerował program do nagrywania.

- Strona wyboru oraz dodawania obszaru:

![first_page_view](https://i.im.ge/2022/10/04/1f7kCm.first-screen.gif)

https://i.im.ge/2022/10/04/1f7kCm.first-screen.gif - jeśli nie wyświetla.

- Strona główna obszaru z poziomu osoby niezalogowanej:

![home_page_view](https://i.im.ge/2022/10/04/1fDovy.second-view.gif)

https://i.im.ge/2022/10/04/1fDovy.second-view.gif - jeśli nie wyświetla.

- Strona główna obszaru z poziomu administratora:

![home_page_view_from_admin](https://i.im.ge/2022/10/04/1fDdMX.home-page.gif)

https://i.im.ge/2022/10/04/1fDdMX.home-page.gif - jeśli nie wyświetla.

- Wszystkie strony z poziomu administratora:

![all_pages_view_from_admin](https://i.im.ge/2022/10/04/1fDIND.generally.gif)

https://i.im.ge/2022/10/04/1fDIND.generally.gif - jeśli nie wyświetla.


## Instalacja
Działanie aplikacji na swoim komputerze można sprawdzić na dwa sposoby:
1) Uruchomienie skompilowanej aplikacji, która łączy się z serwerem SQL 
postawionym na darmowym hostingu. Skutkiem tego jest dłuższe ładowanie okien aplikacji.

Aby to zrobić należy pobrać folder:``..\TablicaDIM\TablicaDIM\bin\Debug\net6.0-windows`` 

Następnie należy uruchomić plik:
`TablicaDIM.exe`


2) Wygenerowanie bazy danych na lokalnym serwerze SQL.

Backup bazy danych znajduję się w folderze głownym. Jego nazwa to `DimTab.bak`.

Po wygnerowaniu bazy należy zmienić connection string na lokalny:
`optionsBuilder.UseSqlServer(PrivateClass.serverString);`

w pliku:
`\TablicaDIM\DBModels\DimTabContext.cs`

Następnie trzeba skompilować i uruchomić aplikację.

Dane do logowania na przykładowe obszary to:
- poziom właściciela: twlasciciel/twlasciciel
- poziom administratora:  tadministrator/tadministrator
- poziom mistrza: mtestowy/mtestowy
- poziom technika: ttestowy/ttestowy

Po zalogowaniu się na konta zapyta o podanie nowego hasła. Proszę wpisać takie jak przy pierwszym logowaniu.
## Szczegółowy opis aplikacji
Po uruchomieniu aplikacji sprawdza ona:
- połączenie z bazą danych,
- połączenie z zewnętrzynm serwerem czasu, by sprawdzić zgodność daty i czasu,
- kontrola wersji aplikacji.
Gdy któryś z warunków jest nie spełniony informuje użytkownika i wyłącza aplikację.

Po przejściu pierwszych króków aplikacja zezwala zaznaczyć opcje:
- Automatyczne odświeżanie aplikacji. - włącza/wyłącza odświeżanie aplikacji o częstotliwości 5 sekund,
- Otwieraj wybrany obszar automatycznie. - po zaznaczeniu tej opcji i kliknięciu w wybrany obszar zapisywany jest wybór użytkownika. Przy następnym uruchomieniu aplikacji włączy on wybrany wcześniej obszar automatycznie. By wyłączyć tę opcję należy cofnąć się do okna wyboru obszaru.

W oknie wyboru obszaru można też stworzyć nowy obszar, bądź wybrać obszar z przeniesionych do
archiwum.

Przy tworzeniu nowego obszaru musimy podać dane nowego obszaru oraz osoby zarządzającej nim. Dostaje ona uprawnienia właściciela, które
dają możliwość zmiany wszystkich opcji.
W późniejszym etapie są też 3 inne poziomy uprawnień (opis poszczególnych okien jest poniżej):
- administrator, który zmniejszony jest o dostęp do okna właściciela obszaru,
- mistrz, który zmniejszony jest o okno właściciela obszaru oraz stanowisk,
- technik, który może tylko:

        1. Dodawać oraz modyfikować akcje, za które jest odpowiedzialny.
        2. Wnioskować o urlopów.
        3. Modyfikować swoje dane.
Po wybraniu obszaru mamy możliwość sprawdzenia strony głownej, wykresów, planu zmian oraz urlopów.

W oknie:
- pracowników -  można dodawać/usuwać/modyfikować pracowników (usuwać, tylko wtedy gdy nie mają powiązania z akcjami),
- stanowisk - można dodawać/usuwać/modyfikować stanowiska (usuwać, tylko wtedy gdy nie mają powiązania z akcjami),
- zmian - ustalać zmiany oraz warunki podświetlania strony głownej,
- pracy w weekend - ustalanie pracy w weekend,
- urlopów - wnioskowanie o urlopy, akeptacja, ustalanie swiąt,
- wykresów - wyświetlania wykresów z poprzednich lat, modyfikowania bieżącego roku,
- administratora - modyfikowanie opcji obszaru.
