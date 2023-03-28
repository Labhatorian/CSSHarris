// de url die cypress om de testen uit te voeren.
const url = 'https://localhost:7194/Chat/Global'
    // Zet de juiste string, hieronder een voorbeeld:
    //'http://localhost:63342/client_toetsing/lessen/week%206%20-%20les%201%20Testing/index.html';


describe('dom spec', () => {
    it('contains text "Global Chat"', () => {
        cy.visit(url)

        cy.contains('Global Chat')
    })
})
