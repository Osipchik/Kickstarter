import React, { useState } from 'react';
import { CompanyTabs } from './CompanyTabs/CompanyTabs';


export const Company = (props) => {
    const [companyId, setCompanyId] = useState(props.match.params.id)

    return(
        <CompanyTabs
            isPreview={false}
            id={companyId}
            // editor={this.props.editor}
            // basic={this.props.basic}
            // founding={this.props.founding}
        />
        // <section>
        //     <p>company page</p>
        //     <p>{this.state.companyId}</p>
        // </section>
    );
}