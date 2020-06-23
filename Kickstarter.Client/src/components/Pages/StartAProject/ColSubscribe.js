import React from "react";

export default function ColSubscribe(props) {
    return(
        <div className="col-5 d-none d-sm-block">
            <h2 className="title-small mx1 mt1 mb-0">{props.title}</h2>
            {props.description.map((item, idx) => (
                <p className="m1 text-description text-input-dark" key={idx}> {item}</p>
            ))}
        </div>
    )
}