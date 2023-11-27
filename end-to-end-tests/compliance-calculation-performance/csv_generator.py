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
country = ['GB-SCT', 'GB-WLS', 'GB-CYM', 'GB-ENG']
fuels = ['Petrol', 'Diesel', 'LPG', 'Electric', 'Petrol-Hybrid']

with open(output_filename, 'w', newline='') as file:
    wr = csv.writer(file, delimiter=',', quotechar='"', quoting=csv.QUOTE_MINIMAL)

    # add headers
    wr.writerow(["vin", "vfn", "Mh", "Man", "MMS", "TAN", "T", "Va", "Ve", "Mk", "Cn", "Ct", "Cr", "M", "MT", "MRVL", "Ewltp", "TPMLM", "W",
                  "At1", "At2", "Ft", "Fm", "Ec", "Z", "IT", "Erwltp", "ber", "dofr", "scheme_year", "postcode", "spvc", "wrm", "mnp", "rlce", "fa", "trrc",
                  "registered_in_nation"])

    # generate faked vehicle data
    for i in range(number_of_records):
        wr.writerow([
            fake.vin(),                                     # vin
            fake.random_number(digits=25,fix_len=True),     # vfn
            fake.company(),                                 # Mh
            fake.vehicle_make(),                            # Man
            fake.company(),                                 # MMS
            fake.bothify(text='?##*####/####*####*##'),     # TAN
            fake.vehicle_category(),                        # T
            fake.bothify(text='???????????'),               # Va
            fake.bothify(text='???????????'),               # Ve
            fake.vehicle_model(),                           # Mk
            fake.company(),                                 # Cn
            random.choice(type_approvals),                  # Ct
            fake.bothify(text='???????????'),               # Cr
            fake.random_number(digits=4,fix_len=True),      # M
            fake.random_number(digits=4,fix_len=True),      # MT
            fake.numerify(text='#%%'),                      # MRVL
            fake.numerify(text='#%%'),                      # Ewltp
            fake.random_number(digits=4,fix_len=True),      # TPMLM
            fake.random_number(digits=4,fix_len=True),      # W
            fake.random_number(digits=3,fix_len=True),      # At1
            fake.random_number(digits=3,fix_len=True),      # At2
            random.choice(fuels),                           # Ft
            fake.lexify(text='?'),                          # Fm
            fake.random_number(digits=4,fix_len=True),      # Ec
            fake.random_number(digits=2,fix_len=False),     # Z
            fake.text(max_nb_chars=200),                    # IT
            fake.random_number(digits=2,fix_len=False),     # Erwltp
            fake.numerify(text='#%%'),                      # ber
            fake.date(),                                    # dofr
            2023,                                           # scheme_year
            fake.postcode(),                                # postcode
            fake.bothify(text='???????????'),               # spvc
            fake.boolean(chance_of_getting_true=95),        # wrm
            fake.numerify(text='#%%'),                      # mnp
            random.choice(rlce),                            # rlce
            fake.numerify(text='#%%'),                      # fa
            fake.lexify(text='?'),                          # trrc
            random.choice(country),                         # registered_in_nation
        ])
