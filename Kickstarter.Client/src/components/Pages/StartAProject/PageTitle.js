import React from 'react'
import Divider from "@material-ui/core/Divider";
import PropTypes from 'prop-types';

export default function PageTitle(props) {
    return(
        <div>
            <br/>
            <div className="text-center">
                <h1 className="title-large ">{props.title}</h1>
                <p className="sub-title-large text-input-dark">{props.subtitle}</p>
            </div>
            <br/>
            <Divider className="mt-4"/>
        </div>
    )
}

PageTitle.propTypes = {
    title: PropTypes.string,
    subtitle: PropTypes.string
};