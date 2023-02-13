# ClientServerSecurityWindesheim
![Platform](https://img.shields.io/badge/platform-web-lightgrey)
![GitHub](https://img.shields.io/github/license/Labhatorian/CSSWindesheim)
![GitHub repo size](https://img.shields.io/github/repo-size/Labhatorian/CSSWindesheim)
![GitHub last commit](https://img.shields.io/github/last-commit/Labhatorian/CSSWindesheim)
![Maintenance](https://img.shields.io/maintenance/yes/2023)<br>

## Structure
ClientTech contains the HTML assignments on a week-to-week basis</br>
ServerTech contains the C# assignments on a week-to-week basis</br>
Security is empty for now</br>
Overkoepelend will contain the project that merges all the assignments together with additional requirements added</br>

## Requirements
### Userstory 1: Profielpagina
Als gebruiker wil ik het profiel van de developer kunnen inzien

1. Het profiel bevat een overzicht van skills
2. Het profiel bevat een beschrijving/introductie van de developer
3. Het profiel bevat een afbeelding van de developer
    - Meerdere afbeeldingen toegestaan
    - Slideshow met afbeeldingen is toegestaan
#### Niet-functionele requirements
| Vak      | Beschrijving                                                          |
| -------- | --------------------------------------------------------------------- |
| Client   | 4. De gebruikte HTML-tags zijn semantisch waar dit mogelijk is        |
|          | 5. Het design is Mobile First                                         |
|          | 6. Het design is Responsive                                           |
|          | 7. De profiel pagina bevat een GDPR                                   |
|          | 8. De GDPR keuze wordt opgeslagen in een cookie (round trip )         |
|          | 9. GDPR wordt alleen getoond als er geen consent is gegeven           |
|          | 10. Styling GDPR past bij pagina                                      |
| Server   | 11. Er wordt gebruikgemaakt van MVC                                   |
|          | 12. De data op de view wordt getoond met behulp van model binding     |
|          | 13. Iedere request wordt opgeslagen als ��n pagina view in een cookie |
| Security | 14. ASVS week 1                                                       |

### Userstory 2: Contactpagina 
Als gebruiker wil ik een bericht kunnen sturen aan de developer

1. De pagina bevat de naam van de developer
2. Het formulier bevat een invoer voor het onderwerp
3. Het formulier bevat een invoer voor e-mail
4. Het formulier bevat een text input voor het bericht
5. Wanneer het formulier verstuurd wordt, ontvangt de developer een mail met de ingevulde gegevens
6. Het formulier bevat een captcha

#### Niet-functionele requirements
| Vak      | Beschrijving                                                          |
| -------- | --------------------------------------------------------------------- |
| Client   | 7. Het formulier wordt alleen verstuurd als de inputs valid zijn       |
|          | 8. Voorwaarden voor de inputs: a. Onderwerp, niet langer dan 200 tekens, b. E-mail, valide emailadres, c. Bericht, niet langer dan 600 tekens                                   |
|          | 9. Het formulier bevat een captcha                                        |
| Server   | 10. Er is een API endpoint beschikbaar die de data uit het formulier afvangt                             |
|          | 11. De data uit het formulier wordt gecontroleerd op een aantal voorwaarden. Deze voorwaarden zijn vastgelegd in een model: a. Onderwerp, niet langer dan 200 tekens, b. E-mail, valide emailadres, c. Bericht, niet langer dan 600 tekens     |
|          | 12. Het endpoint geeft de juiste statuscode terug wanneer de waarden niet voldoen aan de voorwaarden |
|          | 13. De gevalideerde data uit het formulier wordt opgeslagen in een database (maak zelf de keuze tussen een relationele of niet-relationele database) |
|          | 14. Voor het versturen van de mail wordt gebruikgemaakt van een mail delivery service zoals SendGrid |
| Security | 14. ASVS week 1                                                       |
|          |  	16. ASVS week 2 |

### Additional requirements
#### Security
