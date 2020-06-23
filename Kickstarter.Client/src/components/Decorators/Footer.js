import React from "react";
import {containerStyle} from "../Layout";
import Box from "@material-ui/core/Box";
import Typography from "@material-ui/core/Typography";
import Logo from "./Logo/kickstarter-logo-white.webp";
import {Link} from "@material-ui/core";
import {linkStyle} from "./Links";
import '../Styles/Common.css';

export default function Footer() {
    const linkStyles = linkStyle();
    return (
        <footer className="footer">
            <div className={containerStyle}>
                <Typography component="div" className="col-5 d-none d-sm-block">
                    <Box
                        m={1}
                        fontSize="subtitle1.fontSize"
                        fontWeight="fontWeightLight"
                        textAlign="left">
                        Project image
                    </Box>
                </Typography>
                <div className="row">
                    <img src={Logo} alt="logo" height={24} />
                    <Link to="/">About</Link>
                </div>
            </div>
        </footer>
    )
}