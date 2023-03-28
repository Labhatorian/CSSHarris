import getRandomImageUrl from '../../../Helpers/getRandomImageUrl';
import './Card.css';

const Card = (props) => {
    const imageUrl = getRandomImageUrl({ seed: props.name, width: 1200, height: 400 });

    return (
        <button className="card card--active" onClick={props.onClick}>
                    <div className="card__image">
                        <img src={imageUrl} alt="Tatooine" />
                    </div>
                    <div className="card__content">
                        <div className="card__title">
                            {props.name}
                        </div>
                        <div className="card__subtitle">
                            {props.climate.charAt(0).toUpperCase() + props.climate.slice(1)}
                        </div>
                    </div>
                </button>
    );
};

export default Card;