   M S B A M L   `   `   `  ����   	MoviesGUI    MoviesGUI.MovieAdminEdit   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  
Edit Movie��$	��550��$	��777��.��� ���#D9ECFF$��10q� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� -    ��Name$ txtMovieTitleInput��$��Movie Title Here��$��28(�$
��Bold"�$��Center=�$��	0,10,0,20q�$	��400��
�������
���   �$��0��� $��1��$��0,67,0,0q� � ColumnDefinitions �� $��2* ��� $��* �.+��� $��0��$��
10,10,10,0q�$	��Top=�$	��329��.+��� $��Movie Information��$
��Bold"�$��0,0,0,10q�.���� .+��� $��Rental Price:��$��SemiBold"�$��5,5,0,0q��� -   $ txtRentalPriceInput��$��$0.00��$
��10,0q�$	��150���� $��Genre��$
��Bold"�$��0,0,0,10q�.���� -   $ txtGenreInput��$��
Genre Name��$��10q�$	��200���� $��
Actor Info��$
��Bold"�$��0,0,0,10q�$	��157��.���� $	��198��.+��� $��Lead Actor:��$��SemiBold"�$��5q��� -   $ txtLeadActorInput��$��Lead Actor Name��$
��10,0q�$	��200���� $&�� Also Acted In (Comma Separated):��$��SemiBold"�$��5,10,5,5q��� -   $ txtAlsoActedInInput��$��Movie 1, Movie 2��$
��10,0q�$	��300���� $��1��$��10q�$	��Top=�.+��� $��Supplier Info��$
��Bold"�$��0,0,0,10q�.���� .+��� $��	Supplier:��$��SemiBold"�$��5q��� -   $ txtSupplierNameInput��$��Supplier Name��$
��10,0q�$	��200���� $��Email:��$��SemiBold"�$��5q��� -   $ txtSupplierEmailInput��$��supplier@example.com��$
��10,0q�$	��200���� $��Phone:��$��SemiBold"�$��5q��� -   $ txtSupplierPhoneInput��$��+201000000000��$
��10,0q�$	��200���� $��Address��$
��Bold"�.���� .+��� -	   $ txtSupplierStreetInput��$��Street��$��10q��� -
   $ txtSupplierCityInput��$
��City��$��10q��� -   $ txtSupplierPostalInput��$��
PostalCode��$��10q��� -   $ txtSupplierCountryInput��$��Country��$��10q��� $��2��$��Center=�$��10q�.+��� -    ��Name$ btnSaveChanges��$��Save Changes��$	��200��$��45�����#007ACC
�������$
��Bold"�$��16(�