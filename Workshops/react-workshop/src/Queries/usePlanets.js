import { useQuery } from 'react-query';

function usePlanets() {
    // We'll get all the planets from the API here, using this URL: https://swapi.dev/api/planets
    
    /**
     * SWAPI does not provide resources with an 'id'.
     * We can make things a bit easier for ourselves by adding it to every resource with this function.
     * It uses the 'url' property of a resource to determine the 'id'.
     * @param {string} resourceUrl 
     * @returns {string}
     */
    const findIdByResourceUrl = (resourceUrl) => {
        return resourceUrl.match(/\d+/)[0];
    }

    const {data, isFetching} = useQuery('planets', () => {
        return fetch('https://swapi.dev/api/planets').then(response => response.json())
    });

    // To retrieve the data with React Query, we'll be using the 'useQuery' hook. You can find the basics here: https://react-query.tanstack.com/guides/queries 
    // To handle the http call itself, we'll use the Fetch API that JS provides: https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API/Using_Fetch
    const planets =  data?.results.map((planet) => {
        planet.id = findIdByResourceUrl(planet.url);
        return planet
    });

    return {
        planets: planets || [], // Here we'll return the data
        isFetching
    };
}

export default usePlanets;