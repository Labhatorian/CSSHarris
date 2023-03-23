import usePlanets from '../../../Queries/usePlanets';
import Card from '../../Atoms/Card';
import { useNavigate } from 'react-router-dom';
import { Fragment } from 'react';
import './GalleryContainer.css';

// This container will show the gallery page, showcasing the available items to inspect
const GalleryContainer = () => {
    const navigate = useNavigate();
    const {planets, isFetching} = usePlanets();

    const onCardClicked = (planetId) => {
        // Here we direct the user to the details page with 'useNavigate': https://reactrouter.com/docs/en/v6/getting-started/overview
        console.log("Navigate " + planetId)
        navigate('/detail/' + planetId);
    }

    return (
        <div className="container container--centered">
            {
                isFetching ? (
                    <div></div>
                ) : (
                    <Fragment>
                        <h1>In a gallery far, far away...</h1>
                        <div className="gallery-grid">
                        {
                            planets.map((planet) => {
                                return <Card key={planet.id} name={planet.name} climate={planet.climate} onClick={() => { onCardClicked(planet.id)}} />
                            })
                        }
                        </div>
                    </Fragment>
                )
            }
            
        </div>
    );
}

export default GalleryContainer;