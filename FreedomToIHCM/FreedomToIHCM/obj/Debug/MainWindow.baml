   M S B A M L   `   `   `  ����H  DFreedomToIHCM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:FreedomToIHCMFreedomToIHCM  5   	       FreedomToIHCM.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:FreedomToIHCM     5   	     � Title$	  ADP��5   	   $	��550��6   $	��625��6"    � 
ResizeMode$ NoResize=�6.   .��5	      �� $��	20 0 20 5q�6   .+�5
   
   �� $��Freedom to iHCM��6   $��Lucida Sans Typewriter)�6*   $
��Bold"�6N   $��20(�6`   $��Center=�6n   $��Center=�6�   �� 5   
   6   �� 5   
   $��0 10 0 5q�6    �� Orientation$ 
Horizontal=�6'   $��Center=�6@   .+�5      �� $��	Server : ��6   6*   �� 5      $��Stretch=�6   $	��200��64    �� 	MaxLength$ 60��6@   $��Center=�6O   $��Center=�6q   6�   5      � 5   
   -   5         �Name$ 
EntireGrid��5      $��0 10 0 5q�6#    � ColumnDefinitions 5      �� 5      $��5* �6#   �� 5      $��* �6#    � RowDefinitions 5      �� 5      $	��400 �6    .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5   �   "   VerticalScrollBarVisibility$
 Auto=�6   	   CanUserAddRows	 .  
   CanUserDeleteRows
 .     AutoGenerateColumns .     CanUserResizeColumns .  $��Stretch=�6�   $	��Top=�6�   $��0 10 0 5q�6�   $��0��6�   $��0��6     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 S.No��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ 3* 67      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   �� *Id+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5        5      $ From Freedom��6)   $	 10* 6?    6K   �� *FromDatabase+5        5       $ To iHCM��6)   $	 10* 6:    6F   �� *
ToDatabase+5!      4  .System.Windows.Controls.DataGridTemplateColumn  5$      $ 5* 6-      CellTemplate 5%      �� 5&      .R�5'   "   �� -   5        $��Remove��5'   ;   6O   5(      5*        5+      $ Status��6)   $ 5* 69    .  6D   �� *Status+5,      5.      �� 5/      $��0��6   $��1��6&   $	��Top=�67   .+�50      �� -   5        $��
10 50 10 5q�50      $	��Add��6-   ��6J   �� *
AddCommand+�� 51      -   5        $��	10 5 10 5q�51      $��Start��6,   ��6Q   �� *StartCommand+52      53      54      55      