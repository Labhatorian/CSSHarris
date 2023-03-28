import axios from 'axios';

export function retrieveArrivals(airport: string, begin: number, end: number) {
    //return axios.get(`https://opensky-network.org/api/flights/arrival?airtport=${airport}&begin=${begin}&end=${end}`, {
    //   data:{ 
    //        username: 'eleina',
    //        password: 'qgt0GTJ5qtx-kfm.kxy'
     //   }
    //});

    return Promise.resolve({
        data: [
            {
                arrivalTime: '12:00',
                flightNumber: 'KL7010',
                arrivalDate: '2023-03-21',
                airport: 'Schiphol'
            }
        ]
    })

    // URL https://opensky-network.org/api/flights/arrival?airport=EDDF&begin=1616454000&end=1616540400
    // username: 'eleina'
    // password: 'qgt0GTJ5qtx-kfm.kxy'
}