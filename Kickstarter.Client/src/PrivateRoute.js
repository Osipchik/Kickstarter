import React from "react";
import {Route, Redirect} from "react-router-dom";

export const PrivateRoute = ({component: Component, user, ...rest}) => {

    return(
        <Route
            {...rest}
            render={
                (props) => !user || user.expired
                    ? <Redirect to={{pathname: '/'}}/>
                    : <Component {...props}/>
            }
        />
    )
};
