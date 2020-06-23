import React from 'react';
import PropTypes from 'prop-types';
import { CompanyCard } from "./CompanyCard";
import { withRouter } from 'react-router-dom';

export const CompaniesCard = withRouter(props => {
    const handleClick = companyId => props.history.push(`/Company/${companyId}`);

    const categoryClick = category => {
        props.history.push(`/Explore/${category.categoryId}/${category.id}`)
    };

    return (
        <div className="d-flex flex-wrap justify-content-center">
            {props.companies.map(company =>
                <div className="p-2" key={company.id} id={company.id}>
                    <CompanyCard
                        value={company}
                        onClick={() => handleClick(company.id)}
                        onCategoryClick={(category) => categoryClick(category)}
                    />
                </div>
            )}
        </div>
    );
})

CompaniesCard.propTypes = {
    companies: PropTypes.array.isRequired
};
