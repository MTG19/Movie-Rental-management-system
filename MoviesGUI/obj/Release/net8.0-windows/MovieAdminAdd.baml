   M S B A M L   `   `   `  ����   	MoviesGUI#    MoviesGUI.AddMovieAdminWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Add New Movie��$	��650��$	��850��.��� ���#D9ECFF$��10q� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� -    ��Name$ txtMovieTitleInput��$��Enter Movie Title��$��24(�$
��Bold"�$��Center=�$��	0,10,0,20q�$	��400��$��0��
�������
���   �� $��1�� � ColumnDefinitions �� $��2* ��� $��* �.+��� $��0��$��10q�.+��� $��Movie Information��$
��Bold"�$��0,0,0,10q�.���� .+��� $��Rental Price:��$��5q��� -   $ txtRentalPriceInput��$��10q�$	��150���� $��Genre��$
��Bold"�$��0,0,0,10q�.���� .+��� $��Genre:��$��5q��� -   $ txtGenre��$��10q�$	��200���� $��
Actor Info��$
��Bold"�$��0,0,0,10q�.���� $	��175��.+��� $��Lead Actor:��$��5q��� -   $ txtLeadActor��$��10q�$	��200���� $��Also Acted In:��$��	10,10,5,5q��� -   $ txtAlsoActedIn��$��10q�$	��200���� $��1��$��10q�.+��� $��Supplier Info��$
��Bold"�$��0,0,0,10q�.���� .+��� $��Supplier Name:��$��5q��� -   $ txtSupplierName��$��10q�$	��200���� $��Email:��$��5q��� -   $ txtSupplierEmail��$��10q�$	��200���� $��Phone:��$��5q��� -   $ txtSupplierPhone��$��10q�$	��200���� $��Address��$
��Bold"�.���� .+��� -	   $ txtAddressStreet��$��10q�$��Street��
��������� -
   $ txtAddressCity��$��10q�$
��City��
��������� -   $ txtAddressPostal��$��10q�$��Postal Code��
��������� -   $ txtAddressCountry��$��10q�$��Country��
��������� $��2��$��Center=�$��20q�.+��� -    ��Name$ btnAddMovie��$��	Add Movie��$	��200��$��45�����#28A745
�������$��16(�$
��Bold"�