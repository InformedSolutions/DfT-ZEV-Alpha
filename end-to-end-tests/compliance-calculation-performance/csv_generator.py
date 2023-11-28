from sys import argv
import csv
import random
from faker import Faker
from faker_vehicle import VehicleProvider

fake = Faker(['en-GB'])
fake.add_provider(VehicleProvider)

output_filename = argv[1]
number_of_records = int(argv[2])

percent_registered_category = 2
percent_zev = 25
percent_msv = 5
percent_spvc = 5
percent_eco_innovations = 5

sample_details = [
    ['Golf', 'Volkswagen', 'Volkswagen Group', 'Porsche SE'],
    ['A3', 'Audi', 'Volkswagen Group', 'Porsche SE'],
    ['Kamiq', 'Škoda Auto', 'Volkswagen Group', 'Porsche SE'],
    ['Puma', 'Ford', 'Ford of Britain', 'Ford Motor Company'],
    ['Qashqai', 'Nissan UK', 'Nissan Motor Co., Ltd.', 'Groupe Renault'],
    ['Corsa', 'Vauxhall', 'Vauxhall Motors Limited', 'Stellantis'],
    ['Sportage', 'Kia Europe', 'Kia Corporation', '	Hyundai Motor Company'],
    ['Model Y', 'Tesla', 'Tesla, Inc.', ''],
    ['Tucson', ' Hyundai', '', ''],
    ['Juke', 'Nissan UK', 'Nissan Motor Co., Ltd.', 'Groupe Renault'],
    ['Mini Hatch', 'BMW', 'Bayerische Motoren Werke AG', '']
]

type_approvals = ['M1', 'N1', 'N2']
country = ['GB', 'NI']
fuels = ['PETROL', 'DIESEL', 'ELECTRICITY', 'HYBRID ELECTRIC',  'ELECTRIC DIESEL', 'GAS', 'GAS/PETROL', 'FUEL CELLS', 'STEAM', 'OTHER']
spvcs = ['SA', 'SB', 'SC', 'SD', 'SH', 'SG', 'SM']

with open(output_filename, 'w', newline='') as file:
    wr = csv.writer(file, delimiter=',', quotechar='"', quoting=csv.QUOTE_MINIMAL)

    # add headers
    wr.writerow(["vin", "vfn", "eu_manufacturer_name", "uk_registry_manufacturer_name", "oem_manufacturer_name", "type_approval_number", "type", "variant", "version", "make", "model", 
                 "type_approval_category", "registered_category", "mass_in_running_order", "wltp_test_mass", "mass_representitive_of_vehicle_load", "monitoring_mass", 
                 "wltp_c02_emissions", "technically_permissible_maximum_laden_mass", "wheelbase", "axle_track_1", "axle_track_2", "fuel_type", "fuel_mix",
                 "engine_capacity", "electric_energy_consuption", "eco_innovations", "eco_emissions_reduction", "battery_electric_range", "date_of_first_registration",
                 "scheme_year", "postcode", "special_purpose_vehicle_category", "road_load_coefficient_f0", "road_load_coefficient_f1", 
                 "road_load_coefficient_f2", "frontal_area", "tyre_rolling_resistance_class", "registration_location"])
    
    # generate faked vehicle data
    for i in range(number_of_records):
        vehicle_base = random.randint(0, 10)
        
        if random.randint(0,100) > percent_msv:
            monitoring_mass = None
            mass_representitive_of_vehicle_load = None
        else:
            monitoring_mass = fake.random_number(digits=4,fix_len=False)
            mass_representitive_of_vehicle_load = fake.random_number(digits=4,fix_len=False)

        if random.randint(0,100) > percent_registered_category:
            registered_category = None
        else:
            registered_category = random.choice(type_approvals)

        if random.randint(0,100) > percent_zev:
            wltp_c02_emissions = 0
            battery_electric_range = random.randint(20,350)
        else:
            wltp_c02_emissions = fake.random_number(digits=3,fix_len=False)
            battery_electric_range = None

        if random.randint(0,100) > percent_spvc:
            special_purpose_vehicle_category = None
        else:
            special_purpose_vehicle_category = random.choice(spvcs)

        if random.randint(0,100) > percent_eco_innovations:
            eco_innovations = None
        else:
            eco_innovations = fake.text(max_nb_chars=30)


        wr.writerow([
            fake.vin(),                                     # vin
            fake.random_number(digits=25,fix_len=True),     # vfn
            sample_details[vehicle_base][3],                # eu_manufacturer_name
            sample_details[vehicle_base][2],                # uk_registry_manufacturer_name
            sample_details[vehicle_base][1],                # oem_manufacturer_name
            fake.bothify(text='?##*####/####*####*##'),     # type_approval_number
            fake.vehicle_category(),                        # type
            fake.bothify(text='???'),                       # variant
            fake.bothify(text='???'),                       # version
            sample_details[vehicle_base][1],                # make
            sample_details[vehicle_base][0],                # model
            random.choice(type_approvals),                  # type_approval_category
            registered_category,                            # registered_category
            fake.random_number(digits=4,fix_len=False),     # mass_in_running_order
            fake.random_number(digits=4,fix_len=False),     # wltp_test_mass
            mass_representitive_of_vehicle_load,            # mass_representitive_of_vehicle_load
            monitoring_mass,                                # monitoring_mass
            wltp_c02_emissions,                             # wltp_c02_emissions
            fake.random_number(digits=4,fix_len=False),     # technically_permissible_maximum_laden_mass
            fake.random_number(digits=4,fix_len=False),     # wheelbase
            fake.random_number(digits=3,fix_len=False),     # axle_track_1
            fake.random_number(digits=3,fix_len=False),     # axle_track_2
            random.choice(fuels),                           # fuel_type
            fake.lexify(text='?'),                          # fuel_mix
            fake.random_number(digits=4,fix_len=False),     # engine_capacity
            fake.random_number(digits=2,fix_len=False),     # electric_energy_consuption
            eco_innovations,                                # eco_innovations
            fake.random_number(digits=2,fix_len=False),     # eco_emissions_reduction
            battery_electric_range,                         # battery_electric_range
            fake.date(),                                    # date_of_first_registration
            random.choice(country),                         # registration_location
            fake.postcode(),                                # postcode
            special_purpose_vehicle_category,               # special_purpose_vehicle_category
            fake.random_number(digits=2,fix_len=False),     # road_load_coefficient_f0
            fake.random_number(digits=2,fix_len=False),     # road_load_coefficient_f1
            fake.random_number(digits=2,fix_len=False),     # road_load_coefficient_f2
            fake.numerify(text='#%%'),                      # frontal_area
            fake.lexify(text='?'),                          # tyre_rolling_resistance_class
        ])
