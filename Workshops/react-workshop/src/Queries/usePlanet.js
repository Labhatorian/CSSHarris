import { useQuery } from 'react-query';

function usePlanet(planetId) {
    /**
     * We'll get the information for a specific planet here.
     * For this workshop, we want to use the id of the planet and request the data from https://swapi.dev/api/planets/{{id}}
    */

    const {data} = useQuery('planet-' + planetId, () => {
        return fetch('https://swapi.dev/api/planets/' + planetId).then(response => response.json())
    });

    return {
        planet: data// Here we'll return the data
    }
}

export default usePlanet;
