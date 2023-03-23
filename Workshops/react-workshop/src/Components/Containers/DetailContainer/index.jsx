import getRandomImageUrl from '../../../Helpers/getRandomImageUrl';
import usePlanet from '../../../Queries/usePlanet';
import Tab from '../../Tab';
import { useParams, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import './DetailContainer.css'
import DetailContainerContent from './DetailContainerContent';

// This container will hold the contents of the details page, displaying information of the item selected on the gallery page. 
const DetailContainer = () => {
    const {planetId} = useParams();
    const navigate = useNavigate();
    const data = usePlanet(planetId);
    const planet = data?.planet; 

    const [activeTab, setActiveTab] = useState('details');

    const title = planet?.name;
    const imageUrl = getRandomImageUrl({ seed: title, width: 1200, height: 400 });

    const tabs = [
        {
            title: 'Details',
            alias: 'details'
        },
        {
            title: 'People',
            alias: 'people'
        },
        {
            title: 'Movies',
            alias: 'movies'
        }
    ];

    const onTabClicked = (alias) => {
        setActiveTab(alias);
    };

    const goBack = () => {
        navigate('/');
    };

    return (
        <div className="container">
            <button onClick={goBack}>{'<'}</button>
            <h1 className="text-center">{ title }</h1>
            <div className='detail-container'>
                <img className="detail-image" src={ imageUrl } alt={ title || 'planet' } />
                <div className="tabs">
                    {tabs.map((tab) => {
                        return <Tab key={tab.alias} isActive={ activeTab === tab.alias } title={tab.title} onClick={() => {onTabClicked(tab.alias)}}/>;
                    })}
                    {/*<Tab isActive title="Details"/>
                    <Tab title="People"/>
                    <Tab title="Movies"/>
                    <button className="tab tab--active">Details</button>
                    <button className="tab">People</button>
                    <button className="tab">Movies</button>*/}
                </div>
                <DetailContainerContent planet={planet} activeTab={activeTab}/>
                <div className="detail-footer">
                    <button className="button">
                        Take me there!
                    </button>
                </div>
            </div>
        </div>
    );
}

export default DetailContainer;