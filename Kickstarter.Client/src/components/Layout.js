import React, { Component } from 'react';
import NavMenu from './Decorators/NavMenu/NavMenu';
import {ScrollTop} from "./Decorators/NavMenu/ScrollEffect";
import Fab from "@material-ui/core/Fab";
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp';
import CssBaseline from "@material-ui/core/CssBaseline";

export const containerStyle = "col-sm-12 col-md-12 col-lg-11 col-xl-9";

export class Layout extends Component {

    render () {    
        return (
            <div>
                <CssBaseline />
                <NavMenu />
                {this.props.children}
                <ScrollTop {...this.props}>
                    <Fab color="primary" size="medium" aria-label="scroll back to top">
                        <KeyboardArrowUpIcon />
                    </Fab>
                </ScrollTop>
            </div>
        );
    }
}
