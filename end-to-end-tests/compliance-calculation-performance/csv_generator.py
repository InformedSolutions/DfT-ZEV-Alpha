from sys import argv
import csv
import random
from faker import Faker
from faker_vehicle import VehicleProvider

fake = Faker(['en-GB'])
fake.add_provider(VehicleProvider)

output_filename = argv[1]
number_of_records = int(argv[2])

type_approvals = ['N1', 'N2', 'M1', 'M2']
rlce = ['f0','f1','f2']
country = ['GB', 'NI']
fuels = ['Petrol', 'Diesel', 'LPG', 'Electric', 'Petrol-Hybrid']

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
        wr.writerow([
            fake.vin(),                                     # vin
            fake.random_number(digits=25,fix_len=True),     # vfn
            fake.company(),                                 # eu_manufacturer_name
            fake.company(),                                 # uk_registry_manufacturer_name
            fake.numerify(text='#%%'),                      # oem_manufacturer_name
            fake.bothify(text='?##*####/####*####*##'),     # type_approval_number
            fake.vehicle_category(),                        # type
            fake.bothify(text='???????????'),               # variant
            fake.bothify(text='???????????'),               # version
            fake.vehicle_model(),                           # make
            fake.company(),                                 # model
            random.choice(type_approvals),                  # type_approval_category
            fake.bothify(text='???????????'),               # registered_category
            fake.random_number(digits=4,fix_len=True),      # mass_in_running_order
            fake.random_number(digits=4,fix_len=True),      # wltp_test_mass
            fake.numerify(text='#%%'),                      # mass_representitive_of_vehicle_load
            fake.numerify(text='#%%'),                      # monitoring_mass
            fake.numerify(text='#%%'),                      # wltp_c02_emissions
            fake.random_number(digits=4,fix_len=True),      # technically_permissible_maximum_laden_mass
            fake.random_number(digits=4,fix_len=True),      # wheelbase
            fake.random_number(digits=3,fix_len=True),      # axle_track_1
            fake.random_number(digits=3,fix_len=True),      # axle_track_2
            random.choice(fuels),                           # fuel_type
            fake.lexify(text='?'),                          # fuel_mix
            fake.random_number(digits=4,fix_len=True),      # engine_capacity
            fake.random_number(digits=2,fix_len=False),     # electric_energy_consuption
            fake.text(max_nb_chars=200),                    # eco_innovations
            fake.random_number(digits=2,fix_len=False),     # eco_emissions_reduction
            fake.numerify(text='#%%'),                      # battery_electric_range
            fake.date(),                                    # date_of_first_registration
            2023,                                           # scheme_year
            fake.postcode(),                                # postcode
            fake.bothify(text='???????????'),               # special_purpose_vehicle_category
            random.choice(rlce),                            # rlce
            fake.numerify(text='#%%'),                      # frontal_area
            fake.lexify(text='?'),                          # tyre_rolling_resistance_class
            random.choice(country),                         # registration_location
        ])
