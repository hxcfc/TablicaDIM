   M S B A M L   `   `   `  ????E  ATablicaDIM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:TablicaDIM.OtherClassesTablicaDIM.OtherClasses  5      9!clr-namespace:TablicaDIM.ConvertsTablicaDIM.Converts  5      C&clr-namespace:TablicaDIM.View.HolidaysTablicaDIM.View.Holidays  5      2    ,TablicaDIM.View.Holidays.HolidaysApplication   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >OtherClasses%clr-namespace:TablicaDIM.OtherClasses     5      6converts!clr-namespace:TablicaDIM.Converts     5      8local&clr-namespace:TablicaDIM.View.Holidays     5      _ [MaterialDesignThemes.Wpf, Version=4.5.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  6C     ? 	Resources  5      +   %TablicaDIM.Converts.BoleanToPLConvert   BoleanToPLConvert+   %TablicaDIM.Converts.BoleanToLPConvert  BoleanToLPConvert??%:   &	        &	        5   
   6%     5   
   6%   5      .??5      ?? $??10q?6   .+?5   
   ?  ? RowDefinitions 5      ?? 5      $??* ?6    ?? 5      $??* ?6    .+?5      ?? $??Center=?6   $??10 10 10 30q?68   $	??850??6M   .??5      &   System.Windows.Controls.DataGrid  $??Center=?6    ? Cursor$
 Hand??68    ? ToolTip$2 ,Kliknij w nagłówek, aby posortować wolne.??6F      SelectionMode$ Single=?5   g      SelectionUnit$ FullRow=?6~     dataGridStyle#??[ 6?    ?? SelectedItem 6   ?? *HolidaysListSelect+ ?? UpdateSourceTrigger$ PropertyChanged=???5      ?? *ContextToDatagrid+   Columns 5      1  +MaterialDesignThemes.Wpf.DataGridTextColumn  5        DatagridTextBlockStyle1  +System.Windows.Controls.DataGridBoundColumn	   ElementStyle#	 [ 6<   ,  &System.Windows.Controls.DataGridColumn
   Header$
 ID??6?      MaxWidth$ 65Q?6?      MinWidth$ 65Q?6?      Width5  /System.Windows.Controls.DataGridLengthConverter$ 65 6?      Binding 6t   ?? *UnavailableId+6?     5      #	 [ 6<   $
 Nazwisko??6?   $	 200Q?6?   $	 200Q?6?   $	 200 6?    6t   ?? *	Surname+6?     5      #	 [ 6<   $
 Imię??6?   $	 200Q?6?   $	 200Q?6?   $	 200 6?    6t   ?? *Name+6?     5      #	 [ 6<   $
 Data rozpoczęcia wolnego??6?   $	 210Q?6?   $	 210Q?6?   $	 210 6?    6t   ?? *
AbsentFrom+ ?? StringFormat$ {0:dd/MM/yyyy}??6?     5      #	 [ 6<   $
 Data zakończenia wolnego??6?   $	 210Q?6?   $	 210Q?6?   $	 210 6?    6t   ?? *
AbsentTo+$ {0:dd/MM/yyyy}??6?     5      #	 [ 6<   $
 Ilość dni roboczych??6?   $	 140Q?6?   $	 140Q?6?   $	 140 6?    6t   ?? *	DaysCount+6?     5       #	 [ 6<   $
 Powód??6?   $	 140Q?6?   $	 140Q?6?   $	 140 6?    6t   ?? *Reason+6?     5!      #	 [ 6<   $
 Data złożenia??6?   $	 180Q?6?   $	 180Q?6?   $	 180 6?    6t   ?? *
DataOfSend+$ {0:dd/MM/yyyy}??6?     5"      #	 [ 6<   $
 Wniosek zatwierdzenia??6?   $	 170Q?6?   $	 170Q?6?   $	 170 6?    6t   ?? *
Accepted+ ?? 	Converter# [  6?     5#      #	 [ 6<   $
 Wniosek usunięcia??6?   $	 210Q?6?   $	 210Q?6?   $ 21 6?    6t   ?? *
ToDelete+# [ 6?      	CellStyle 5%      ?? 5&       ?? 
TargetType*  $System.Windows.Controls.DataGridCell"  6      MaterialDesignDataGridCell ?? BasedOn# [ 6:   .?5'      ??  ?? Property ?@?ToolTip ?? Value$8 2Kliknij, aby otworzyć dodatkowe opcje do wolnego.??68   ?? 5(       ? ??$
 Wrap=?6G   ?? 5)       ?@??TextAlignment$ Center=?6H   ?? 5*       ? ??$ Center=?6I    ?? Triggers 5+      P? 5,   "    P? Property ?@ 
IsSelected P? Value$
 True??6@   . ?5-   &   ??  ? ??  whiteColorStyle# [ 6C   ?? 5.   &    ? ??  firstColorStyle# [ 6C   5/   #   51      53      54      (	  "MaterialDesignThemes.Wpf.ColorZone	  55       	  Mode$ 
PrimaryMid=?6l   $??1??6~   y?6'   ?? *	VisOption+$ PropertyChanged=?.??56      ??  ?? Padding$ 16q?6   $ ??Decyzja dotycząca wniosku??6)   57      ?? 58      #??[ 6|   $) #Kliknij, aby zaakceptować wniosek.??6?    0 ,MaterialDesignFloatingActionMiniAccentButton#??[ 6?   $??Bottom=?6  $??Right=?68  $??0??6T  $??	0 0 0 -20q?6a  ??6   ?? *AcceptCommand+y?67   ?? *	VisOption+$ PropertyChanged=???6t  0
  *MaterialDesignThemes.Wpf.PackIconExtension
   
  Kind$ Check=? 
  Size 22?? 59      #??[ 6}   $% Kliknij, aby odrzucić wniosek.??6?   #??[ 6?   $??Bottom=?6  $??Right=?65  $??0??6Q  $??
0 0 45 -20q?6^  ??6   ?? *DeclineCommand+y?68   ?? *	VisOption+$ PropertyChanged=???6r  
  $ Minus=? 225:      5;      5<      