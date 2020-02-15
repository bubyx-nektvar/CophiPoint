create
or
replace table users
(
    email varchar (150) null,
    full_name varchar (250) null,
    id int auto_increment
    primary key,
    sub varchar (250) not null
);

create
or
replace table purchases
(
    id int auto_increment
    primary key,
    product_id int not null,
    user_id int not null,
    purchuase_date datetime default current_timestamp () not null,
    product varchar (250) not null,
    price decimal not null,
    constraint purchases_users_id_fk
    foreign key (user_id) references users (id)
);

create
or
replace table tokens
(
    id int auto_increment
    primary key,
    user_id int not null,
    created_at datetime default current_timestamp () not null,
    expires_at datetime not null,
    token varchar (512) not null,
    token_type varchar (30) not null,
    constraint tokens_token_uindex
    unique (token),
    constraint tokens_users_id_fk
    foreign key (user_id) references users (id)
);


