   M S B A M L   `   `   `  ���� N  JDeLauncherFormCore6, Version=2.5.0.1, Culture=neutral, PublicKeyToken=null/clr-namespace:DeLauncherFormDeLauncherForm      DeLauncherForm.MainWindow   \ XPresentationFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:DeLauncherForm       � Title$  
DeLauncher��$��600.000��$��800.000�� � WindowStyle$
 None=� � 
ResizeMode$ NoResize=�����  �� ImageSource$ Resources/Main/base.png��.���  � ColumnDefinitions �� $��* �.+��� -    ��Name$ BackGrondImage2��$
��Left=�$	��600��$	��Top=�$	��800��$'��!/Windows/Resources/Main/base2.png��$y�Hidden=��� -   $ InfoAll��$
��Left=�$	��600��$	��Top=�$	��800���� -    ��Name$ launch��$	��140��$��596,65,65,498q�.���� -   $ launchSource��$
��Fill=�$��-3,-3,-1,-2q��� -   $ WorldBuilder��$��596,137,65,428q�.���� -   $ WorldbuilderSource��$
��Fill=�$��-2,-3,-3,-3q��� -   $ 
QuickStart��$��596,208,65,357q�.���� -   $ QuickStartSource��$
��Fill=�$��-3,-3,-3,-3q��� -	   $ Windowed��$��596,279,65,286q�.���� -
   $ WindowedSource��$
��Fill=�$��-3,-3,-3,-3q��� -   $
 Exit��$��596,353,65,212q�.���� -   $ 
ExitSource��$
��Fill=�$��-3,-3,-3,-3q��� -   $ BP��$��572,485,133,73q�.���� -   $ BPSource��$
��Fill=�$��-3,-3,-3,-3q��� -   $ 
BPChanglog��$��667,485,68,73q�.���� -   $ BPChangelogSource��$
��Fill=�$��-3,-3,-3,-3q��� -   $ HP��$��345,485,360,73q�.���� -   $ HPSource��$
��Fill=�$��-3,-3,-3,-3q��� -   $ 
HPChanglog��$��440,485,295,73q�.���� -   $ HPChangelogSource��$
��Fill=�$��-3,-3,-3,-3q��� -   $ Vanilla��$��118,485,522,73q�.���� -   $ VanillaSource��$
��Fill=�$��-3,-3,-3,-3q��� -   $ QSIndicator��$
��Left=�$��62��$��552,194,0,0q�$	��Top=�$��25��$/��)/Windows/Resources/Main/indicator_off.png��$
��Fill=��� -   $ WindowIndicator��$
��Left=�$��62��$��552,265,0,0q�$	��Top=�$��25��$/��)/Windows/Resources/Main/indicator_off.png��$
��Fill=��� -   $ AdvancedOptions��$��183,302,0,0q�$	��Top=�$��36��$
��Left=�$	��103��.���� -   $ OptionsSource��$
��Fill=�$��-3,-3,-3,-3q��� -    ��Name$ VersionInfo��$��	0.00.00.0��
�������$��18(�$��205,370,507,200q�����  �� 
TargetType" ��.R��� ._�� .+��� #��z���� #��z��	 J� Effect	 -  'System.Windows.Media.Effects.BlurEffect  
   Radius$
 5Q��� -   $ ShtoraBP��$
��Left=�$��40��$	��Top=�$��40��$��523,484,0,0q�$(��"/Windows/Resources/Main/shtora.png���� -   $ ShtoraHP��$
��Left=�$��40��$	��Top=�$��40��$��296,484,0,0q�$(��"/Windows/Resources/Main/shtora.png���� -   $ ShtoraVanilla��$
��Left=�$��40��$	��Top=�$��40��$��
69,484,0,0q�$(��"/Windows/Resources/Main/shtora.png���� -   $ ShtoraManual��$
��Left=�$��40��$	��Top=�$��40��$��
69,434,0,0q�$(��"/Windows/Resources/Main/shtora.png���� $
��Left=�$	��228��$��	52,52,0,0q�$	��Top=�$	��425��$'��!/Windows/Resources/Main/frame.png��$
��Fill=� K� ZIndex$ 10��   DeLauncherForm.GifImage  -      Name$ gifImage��$
��None=�$��52,52,323,320q�$
��Auto��$
��Auto���� -!   $	 Eng��$
��Left=� J� Opacity$ 0Q�$��128,304,0,0q�$	��Top=�$��40��$��37���� -"   $	 Rus��$
��Left=�$ 0Q�$��128,354,0,0q�$	��Top=�$��40��$��37���� -#   $ RusImage��$
��Left=�$��46��$��
65,350,0,0q�$	��Top=�$��58��$%��/Windows/Resources/Main/rus.png���� -$   $ EngImage��$
��Left=�$��46��$��
65,300,0,0q�$	��Top=�$��58��$%��/Windows/Resources/Main/eng.png���� $
��Left=�$��27��$��183,369,0,204q�$	��100��$'��!/Windows/Resources/Main/trans.png��$	 0.3Q��� -%   $ 
NoInternet��$��52,52,323,320q�$.��(/Windows/Resources/Main/nointernet_e.png���� -&   $ ManualFileSelect��$��118,435,522,123q�.���� -'   $ ManualFileSelectSource��$
��Fill=�$��-3,-3,-3,-3q��� -(    ��Name$ 	FilesList��$y�Hidden=�$
��Left=�$��87��$��345,437,0,0q�$	��Top=�$	��383��
���   �
���   ����� .R�A�  A� Columns$ 2������  �� 
TargetType" ��.���  �� Property � �� �� Value$	 0.5q���  � ��
 �k�����  � �� �� " ��.R���  ��Name$	 brd��#��z��#��z��#��z��._��� $|�False��#��z��#��z�� d� 	IsChecked ��  �� Path$ 
IsSelected� �� RelativeSource �� *TemplatedParent+ �� Mode$ TwoWay=�.����  �� Triggers P�  P� Property �@��
IsSelected P� Value$
 True��. ���  � ��
 �  ����  � ��
 �������  � ��$ 1q���  � ��
 �   ��� -)   $ NewsTickerBorder��$��0��
���   �$��1q�$��29��
 �� Top$	 436�� �� Left$	 343��$��296,435,68,136q�._��� -*    ��Name$ 
NewsTicker�� �� 
IsReadOnly$
 True�� J� IsHitTestVisible$ False��$
}�True��$��Center=�$��17(�$
��TEST��
�������
������ $��0,0,0,0q�