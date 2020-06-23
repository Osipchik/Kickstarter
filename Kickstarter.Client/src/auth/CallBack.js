import React, { Component } from "react";
import { connect } from "react-redux";
import { CallbackComponent } from "redux-oidc";
import userManager from "./userManager";
import PropTypes from "prop-types";


class CallbackPage extends Component {

    successCallback(user) {
        console.log('=============redirect=============')
        console.log(user);
        window.location.href = '/';
    };
 
    errorCallback(error) {
        console.log('-------------error-------------')
        console.error(error);
    };

    render() {
        return (
            <CallbackComponent
                userManager={userManager} 
                successCallback={this.successCallback} 
                errorCallback={this.errorCallback}
            >
                <div>Redirecting...</div>
            </CallbackComponent>
        )
    }
}

CallbackPage.propTypes = {
    dispatch: PropTypes.func.isRequired
};

export default connect()(CallbackPage);
