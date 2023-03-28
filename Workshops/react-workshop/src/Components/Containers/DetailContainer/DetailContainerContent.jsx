const DetailContainerContent = ({planet, activeTab}) => {
    const renderGeneralInfoTable = () => {
        return(
            <table className="info-table">
                            <tbody>
                                <tr>
                                    <td>Climate:</td>
                                    <td>{planet?.climate}</td>
                                </tr>
                                <tr>
                                    <td>Terrain:</td>
                                    <td>{planet?.terrain}</td>
                                </tr>
                                <tr>
                                    <td>Population:</td>
                                    <td>{planet?.population}</td>
                                </tr>
                                <tr>
                                    <td>Gravity:</td>
                                    <td>{planet?.gravity}</td>
                                </tr>
                            </tbody>
                        </table>
        );
    };

    const renderStatisticsInfoTable = () => {
       return (<table className="info-table">
                            <tbody>
                                <tr>
                                    <td>Diameter:</td>
                                    <td>{planet?.diameter}</td>
                                </tr>
                                <tr>
                                    <td>Rotation period:</td>
                                    <td>{planet?.rotation_period}</td>
                                </tr>
                                <tr>
                                    <td>Orbital period:</td>
                                    <td>{planet?.orbital_period}</td>
                                </tr>
                            </tbody>
                        </table>);
    }

    return activeTab === 'details' ? (
        <div className="detail-content">
                    <div className="detail-content__block">
                        <h2>General</h2>
                        {renderGeneralInfoTable()}
                    </div>
                    <div className="detail-content__block">
                        <h2>Statistics</h2>
                        {renderStatisticsInfoTable()}
                    </div>
                </div>
    ) : activeTab === 'people' ? (
        <div className="detail-content">People</div>
    ) : (
        <div className="detail-content">Movies</div>
    )
};

export default DetailContainerContent;