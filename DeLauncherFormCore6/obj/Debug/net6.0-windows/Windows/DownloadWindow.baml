   M S B A M L   `   `   `  ����N  JDeLauncherFormCore6, Version=2.5.0.1, Culture=neutral, PublicKeyToken=null/clr-namespace:DeLauncherFormDeLauncherForm  5   	   #    DeLauncherForm.DownloadWindow   5   	   \ XPresentationFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:DeLauncherForm     5   	     � Title$  Download in progress...��5   	   $	��600��6)   $	��600��66    � 
ResizeMode$ CanMinimize=�6B   ��5	      �� 5
   
   -   5        
BackGround�� �� ImageSource$ Resources/chinook.png��5
   )   .��5      � .+�5   
   �� -   5         ��Name$
 Info��5      $%��/Windows/Resources/info_eng.png��6   $
��Left=�6G   $	��572��6b   $	��Top=�6o   $	��594��6�   �� 5   
   $�� ��6    �� 
IsReadOnly$
 True��6   $��Tahoma)�6,   $
��Left=�6@   $��30��6[   $��284,382,0,0q�6g    �� TextWrapping$
 Wrap=�6|   $��15(�6�   $	��Top=�6�   $	��246��6�   
�������5      � 5   
   -   5         �Name$ ProgressBar��5       � Minimum$ 0Q�6-    � Maximum$	 100Q�6:   $
��Left=�6H   $��42��6d   $��284,434,0,0q�6p   $	��Top=�6�   $	��246��6�   �� 5   
   -   5        	 ��Name$	 ProgressLine��5      $�� ��6*   $
��Left=�62   $��348,444,0,0q�6M   $
��Wrap=�6b   $��17(�6v   $	��Top=�6�   $	��182��6�   $��31��6�   �� 5   
   -   5        $	 DownloadInfo��5      $
��Left=�6,   $��288,386,0,0q�6G   $�� ��6\   $��15(�6d   $
��Wrap=�6r   $��Tahoma)�6�   $	��Top=�6�   $	��242��6�   $��27��6�   �� 5   
   -   5        
 ��Name$
 CancelDownload��5      $��Cancel download��6)   $��15(�6C   $
��Left=�6Q   $��330,506,0,0q�6l   $	��Top=�6�   $	��154��6�   $��40��6�   6�   5      5      