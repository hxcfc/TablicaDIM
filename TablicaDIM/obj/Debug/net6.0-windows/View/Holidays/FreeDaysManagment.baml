   M S B A M L   `   `   `  ????E  ATablicaDIM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:TablicaDIM.View.HolidaysTablicaDIM.View.Holidays  5      A%clr-namespace:TablicaDIM.OtherClassesTablicaDIM.OtherClasses  5      9!clr-namespace:TablicaDIM.ConvertsTablicaDIM.Converts  5	      0    *TablicaDIM.View.Holidays.FreeDaysManagment   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:TablicaDIM.View.Holidays     5      _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962a:md1http://materialdesigninxaml.net/winfx/xaml/themes  5      8OtherClasses%clr-namespace:TablicaDIM.OtherClasses  5      0converts!clr-namespace:TablicaDIM.Converts  5	        ? 	Resources  5      +   %TablicaDIM.Converts.BoleanToPLConvert   BoleanToPLConvert+   %TablicaDIM.Converts.BoleanToLPConvert  BoleanToLPConvert??%:   &	        &	        5   
   6%     5   
   6%   5      .??5      ?? $??10q?6   .+?5   
   ?? y?6   ?? *
GridView+ ?? UpdateSourceTrigger$ PropertyChanged=?.+?5      ?? $??Center=?6   $??10q?68   $	??800??6D   .??5      &   System.Windows.Controls.DataGrid  $??Center=?6    ? Cursor$
 Hand??68    ? ToolTip$2 ,Kliknij w nagłówek, aby posortować wolne.??6F      SelectionMode$ Single=?5   c      SelectionUnit$ FullRow=?6z     dataGridStyle#??[ 6?    ?? SelectedItem 6   ?? *SelectedPerson+$ PropertyChanged=???5      ?? *ContextToDatagrid+   Columns 5      1  +MaterialDesignThemes.Wpf.DataGridTextColumn  5        DatagridTextBlockStyle1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [ 60   ,  &System.Windows.Controls.DataGridColumn	   Header$	 ID??6?   
   MaxWidth$
 65Q?6?      MinWidth$ 65Q?6?      Width5  /System.Windows.Controls.DataGridLengthConverter$ 65 6?      Binding 6h   ?? *
PersonId+6?     5      # [ 60   $	 Nazwisko??6?   $	
 200Q?6?   $	 200Q?6?   $	 200 6?    6h   ?? *	Surname+6?     5      # [ 60   $	 Imię??6?   $	
 200Q?6?   $	 200Q?6?   $	 200 6?    6h   ?? *Name+6?     5      # [ 60   $	 Login??6?   $	
 150Q?6?   $	 150Q?6?   $	 150 6?    6h   ?? *Login+6?     5      # [ 60   $	 Email??6?   $	
 300Q?6?   $	 300Q?6?   $	 300 6?    6h   ?? *Email+6?     5      # [ 60   $	 Pozostały urlop??6?   $	
 200Q?6?   $	 200Q?6?   $	 200 6?    6h   ?? *
FreeDays+6?     5      # [ 60   $	 Dodany dnia??6?   $	
 200Q?6?   $	 200Q?6?   $	 200 6?    6h   ?? *	AddWhen+6?     5      # [ 60   $	 Dodany przez??6?   $	
 150Q?6?   $	 150Q?6?   $	 150 6?    6h   ?? *AddWho+6?      	CellStyle 5       ?? 5!       ?? 
TargetType*  $System.Windows.Controls.DataGridCell"  6      MaterialDesignDataGridCell ?? BasedOn# [ 6:   .?5"      ??  ?? Property ?@?ToolTip ?? Value$8 2Kliknij, aby otworzyć dodatkowe opcje do wolnego.??68   ?? 5#       ? ??$
 Wrap=?6G   ?? 5$       ?@??TextAlignment$ Center=?6H   ?? 5%       ? ??$ Center=?6I    ?? Triggers 5&      P? 5'   "    P? Property ?@ 
IsSelected P? Value$
 True??6@   . ?5(   &   ??  ? ??  whiteColorStyle# [ 6C   ?? 5)   &    ? ??  firstColorStyle# [ 6C   5*   #   5,      5.      5/      ?? 50      $??10q?6   $??Center=?6&    ?? Orientation$ 
Horizontal=?6C   .+?51      ?? $#??Ustaw wszystkim 26 dni urlopu??6C   $> 8Kliknij, aby nadać wszystkim pracownikom 26 dni urlopu.??6k   ??6   ?? *ResetDaysCoutCommand+6k   ?? 52      $??	10 0 10 0q?6   $??Usuń wszystkim wolne??6`   $: 4Kliknij, aby usunąć wolne wszystkich pracowników.??6   ??6-   ?? * DeleteAllHolidaysOptionCommand+6   ?? 53      $ ??Modyfikuj wolne pracownika??6`   $6 0Kliknij, aby zmienić dane wybranego pracownika.??6?   {?6   ?? *SelectedItem+??6<   ?? *ModClickCommand+6?   54      55      ?? 56   
   $??10q?6[   y?6   ?? *
ChooseItem+$ PropertyChanged=?.+?57      ?? $??Center=?6   $ 
Horizontal=?66   .+?58      ?? $#??Aktualnie wybrany pracownik: ??6   ?? 59      #??[ 6   ??6J   ?? *SelectedPerson.Surname+$ PropertyChanged=??? 5:      $?? ??6   ?? 5;      #??[ 6   ??6J   ?? *SelectedPerson.Name+$ PropertyChanged=??? 5<      $.??(. Poniższe dane dotyczą obecnego roku.??6   5=      ?? 5>      $
??Left=?6   $ 
Horizontal=?64   .+?5?      ?? $%??Pozostała ilość dni urlopu: ??6   ?? 5@      #??[ 6   ??6J   ?? *SelectedPerson.FreeDays+$ PropertyChanged=?5A      ?? 5B      $
??Left=?6   $ 
Horizontal=?64   .+?5C      ?? $"??Liczba użytych dni urlopu: ??6   ?? 5D      #??[ 6   ??6J   ?? *	DaysURLOP+$ PropertyChanged=?5E      ?? 5F      $
??Left=?6   $ 
Horizontal=?64   .+?5G      ?? $#??Liczba użytych dni choroby: ??6   ?? 5H      #??[ 6   ??6J   ?? *DaysCHOROBA+$ PropertyChanged=?5I      ?? 5J      $
??Left=?6   $ 
Horizontal=?64   .+?5K      ?? $ ??Liczba użytych dni krwi: ??6   ?? 5L      #??[ 6   ??6J   ?? *
DaysKREW+$ PropertyChanged=?5M      ?? 5N      $
??Left=?6   $ 
Horizontal=?64   .+?5O      ?? $%??Liczba użytych dni odbiórek: ??6   ?? 5P      #??[ 6   ??6J   ?? *DaysODBIORKA+$ PropertyChanged=?5Q      ?? 5R      $
??Left=?6   $ 
Horizontal=?64   .+?5S      ?? $"??Liczba użytych dni opieki: ??6   ?? 5T      #??[ 6   ??6J   ?? *
DaysOPIEKA+$ PropertyChanged=?5U      ?? 5V      $
??Left=?6   $ 
Horizontal=?64   .+?5W      ?? $#??Liczba użytych dni postoju: ??6   ?? 5X      #??[ 6   ??6J   ?? *
DaysPOSTOJ+$ PropertyChanged=?5Y      ?? 5Z      $
??Left=?6   $ 
Horizontal=?64   .+?5[      ?? $$??Liczba użytych dni szkoleń: ??6   ?? 5\      #??[ 6   ??6J   ?? *DaysSZKOLENIE+$ PropertyChanged=?5]      ?? 5_      $??10q?6   $??Center=?6%   $ 
Horizontal=?6B   .+?5`      ?  ? 	StaysOpen$ False??6   $??Bottom=?6+    ? HorizontalOffset$	 -85??6>    ? VerticalOffset$ 5??6U    ? PlacementTarget 6h   ??  ?? RelativeSource ?? *TemplatedParent+??6?   ?? *PopoutHolidayOpen+$ PropertyChanged=?.??5a      ?? #??[ 6   #??[ 6L   $??1q?6z   ._?5b      ? $??10q?6   .+?5c      ?? $ Vertical=?6*   .+?5d   "   ?? $??Center=?6,      MaterialDesignDarkBackground#??[ 6I   $??Dodaj wolne:??6?   $
??bold"?6?   (	  "System.Windows.Controls.DatePicker	  5e   "    ? Language$ pl-PL?6-   $( "Wybierz datę rozpoczęcia urlopu.??5g   )     standardDatePickerBox#??[ 6T   $??5q?5h   )   )
  #MaterialDesignThemes.Wpf.HintAssist 
  Hint$ Data rozpoczęcia urlopu??65    	  DisplayDateStart 5e   >   ?? *	MinDate+ 	  DisplayDateEnd 6c   ?? *	MaxDate+  	  Text  5f   )   ?? *StartHolidayDate+$ PropertyChanged=?! ?? Mode$! TwoWay=?5h   5   	  5i   "   $ pl-PL?6-   $n hWybierz datę zakończenia urlopu. Pamiętaj, iż nie może być wcześniejsza niż dzień rozpoczęcia.??5k   )   #??[ 6?   $??5q?5l   )   $ Data zakończenia urlopu??65    5i   >   ?? *	MinDate+ 6c   ?? *	MaxDate+  5j   )   ?? *EndHolidayDate+$ PropertyChanged=?$! TwoWay=?5l   5   ?? 5m   "   $??0 13 0 0q?6-   $??Center=?6?   $ 
Horizontal=?6\   .+?5n   &   ?? $??Ilość dni roboczych: ??60   ?? 5o   &   ??60   ?? *DaysOfSelectedDates+$ PropertyChanged=?5p   #   ?? 5q   "   $ Wybierz powód urlopu.??6?   $??13q?6?   $??5q?6?   $ Wybierz powód??6?   " ?? SelectedValue" 6+   ?? *ReasonHoliday+$ PropertyChanged=?$! TwoWay=???6?   ?? *ReasonsOfHolidays+?? 5r   "   $? |Zaznacz jeśli urlop jest w kolejnym roku. Pozwoli to ustawić datę z kolejnego roku oraz nie usunie dnia wolnego z urlopu.??6+   $??10q?6?   $0??*Wniosek o urlop w styczniu kolejnego roku.??6  # d? 	IsChecked# 6?   ?? *NextYearDate+$ PropertyChanged=?6  ?? 5s   "   $??1??6)    	 standardRightButton#??[	 6b   $??Nadaj wolne??6?   $  Kliknij, aby dodać wolne.??6?   ??69   ?? *SubmitHolidayCommand+6?   5u      5v      5w      5x      ? 5y      $ False??6   $??Bottom=?6+   $	 120??6>   $ 5??6U    6h   ??  ?? *TemplatedParent+??6?   ?? *
PopoutOpen+$ PropertyChanged=?.??5z      ?? #??[ 6   #??[ 6L   $??1q?6z   ._?5{      ? $??10q?6   .+?5|      ?? $ Vertical=?6*   .+?5}   "   ?? $??Center=?6,   #??[ 6I   $??Zmiana dni urlopu:??6?   $
??bold"?6?   ?? 5~   "    
 standardTextBox#??[
 6*   $??5q?6S   $" Liczba dni urlopu pracownika??6a   $ ?? 	MaxLength$$ 5??6?   $; 5Wpisz dni urlopu pracownika. Tylko liczby całkowite.??5      ??5~   ?   ?? *NewDaysValue+$ PropertyChanged=?5      ?? 5?   "   $??1??6)   #??[	 6_   $??Zmień ilość urlopu??6?   $( "Kliknij, aby ustawić liczbę dni.??6?   ??69   ?? *SubmitDaysCommand+6?   5?      5?      5?      5?      ?? 5?      $??	10 0 10 0q?6   $??Nadaj urlop??6\   $3 -Kliknij, aby otworzyć okno dodawania urlopu.??6r   ??6-   ?? *OpemPopHolidayClickCommand+6r   ?? 5?      $??	10 0 10 0q?6   $,??&Zmień liczbę pozostałych dni urlopu??6U   $H BKliknij, aby otworzyć okno zmiany liczby dni pozostałego urlopu.??6?   ??6-   ?? *OpemPopClickCommand+6?   ?? 5?      $??	10 0 10 0q?6   $??Usuń wszystkie wolne??6i   $7 1Kliknij, aby usunąć wolne wybranego pracownika.??6?   ??6-   ?? *)'DeleteAllHolidaysForPersonOptionCommand+6?   ?? 5?      $??	10 0 10 0q?6   $&?? Usuń wszystkie wolne w tym roku??6q   $I CKliknij, aby usunąć wolne wybranego pracownika w bieżącym roku.??6?   ??6-   ?? *1/DeleteAllHolidaysForThisYearPersonOptionCommand+6?   5?      ?? 5?      $??0 15 0 0q?6   .+?5?      ?? $??Center=?6   $%??Dni wolne wybranego pracownika:??69   ?? 5?      $??Center=?6   $??10q?6<   $	??400??6H   .??5?        $??Center=?6   $
 Hand??6<   $2 ,Kliknij w nagłówek, aby posortować wolne.??6J   $ Single=?5?   d   $ FullRow=?6{   #??[ 6?    6   ?? *SelectedHoliday+$ PropertyChanged=???5?      ?? *HolidaysToDatagrid+ 5?        5?      # [ 64   $	 ID??6?   $
 65Q?6?   $ 65Q?6?   $ 65 6?    6l   ?? *UnavailableId+6?     5?      # [ 64   $	 Data rozpoczęcia wolnego??6?   $	
 210Q?6?   $	 210Q?6?   $	 210 6?    6l   ?? *
AbsentFrom+% ?? StringFormat$% {0:dd/MM/yyyy}??6?     5?      # [ 64   $	 Data zakończenia wolnego??6?   $	
 210Q?6?   $	 210Q?6?   $	 210 6?    6l   ?? *
AbsentTo+$% {0:dd/MM/yyyy}??6?     5?      # [ 64   $	 Ilość dni roboczych??6?   $	
 140Q?6?   $	 140Q?6?   $	 140 6?    6l   ?? *	DaysCount+6?     5?      # [ 64   $	 Powód??6?   $	
 140Q?6?   $	 140Q?6?   $	 140 6?    6l   ?? *Reason+6?     5?      # [ 64   $	 Data złożenia??6?   $	
 180Q?6?   $	 180Q?6?   $	 180 6?    6l   ?? *
DataOfSend+$% {0:dd/MM/yyyy}??6?     5?      # [ 64   $	 Wniosek zatwierdzenia??6?   $	
 170Q?6?   $	 170Q?6?   $	 170 6?    6l   ?? *
Accepted+& ?? 	Converter#& [  6?     5?      # [ 64   $	 Wniosek usunięcia??6?   $	
 210Q?6?   $	 210Q?6?   $ 21 6?    6l   ?? *
ToDelete+#& [ 6?    5?      ?? 5?      "  6$   # [ 6>   .?5?   "   ??  ?@?ToolTip$8 2Kliknij, aby otworzyć dodatkowe opcje do wolnego.??6<   ?? 5?   "    ? ??$
 Wrap=?6K   ?? 5?   "    ?@??TextAlignment$ Center=?6L   ?? 5?   "    ? ??$ Center=?6M    5?   "   P? 5?   &    ?@ 
IsSelected$
 True??6D   . ?5?   *   ??  ? ??# [ 6G   ?? 5?   *    ? ??# [ 6G   5?   '   5?      5?      5?      5?      ?? 5?      $??Center=?6   $ 
Horizontal=?66   .+?5?      ?? $??
10 20 10 0q?6   $??Powrót??6S   $: 4Kliknij, aby powrócić do strony wyboru pracownika.??6d   ??6.   ?? *BackClickCommand+6d   ?? 5?      $??
10 20 10 0q?6h   $??Usuń wybrane wolne??6?   $5 /Kliknij, aby usunąć wybrane wolne pracownika.??6?   {?6   ?? *SelectedHolidayBool+$ PropertyChanged=???6|   ?? *DelClickCommand+6?   5?      5?      5?      5?      