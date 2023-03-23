const Tab = (props) => {
    const classes = 'tab' + (props.isActive ? " tab--active" : "")

    return <button className={classes} onClick={props.onClick}>{props.title}</button>
};

export default Tab;