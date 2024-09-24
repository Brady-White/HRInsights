import React, { useState } from "react";
import {
  Box,
  Text,
  Avatar,
  Stack,
  Input,
  Button,
  Flex,
  useToast,
} from "@chakra-ui/react";
import axios from "../api/axios"; //
import Cookies from "js-cookie"; //

export default function EmployeeCard() {
  const [name, setName] = useState("");
  const [salary, setSalary] = useState("");
  const [avatarUrl, setAvatarUrl] = useState("");
  const [searchTerm, setSearchTerm] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const toast = useToast();

  // Function to handle search
  const handleSearch = async () => {
    setIsLoading(true);
    try {
      const response = await axios.get(`api/employee?query=${searchTerm}`, {
        headers: {
          Authorization: `Bearer ${Cookies.get("token")}`,
        },
      });
      console.log(response.data);

      // Assuming the response contains employee data
      const employee = response.data.employee;
      setName(employee.firstName + " " + employee.lastName);
      setSalary(employee.salary);
      setAvatarUrl(employee.avatarUrl);

      toast({
        title: "Employee found.",
        status: "success",
        duration: 3000,
        isClosable: true,
      });
    } catch (error) {
      console.log(error);
      toast({
        title: response.data.message || "An error occurred.",
        description: "Please try again.",
        status: "error",
        duration: 3000,
        isClosable: true,
      });
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Box>
      {/* Search bar */}
      <Flex justifyContent="center" mb={8}>
        <Input
          placeholder="Search for an employee..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          width="300px"
          bg="white"
          border="2px solid"
          borderColor="gray.300"
          borderRadius="lg"
          _hover={{ borderColor: "gray.400" }}
          _focus={{ borderColor: "#ffc808", boxShadow: "0 0 0 1px #ffc808" }}
        />
        <Button
          onClick={handleSearch}
          isLoading={isLoading}
          loadingText="Searching..."
          ml={4}
          bg="#ffc808"
          color="white"
          _hover={{ bg: "#e0b707" }}
        >
          Search
        </Button>
      </Flex>

      {/* Show employee details after search */}
      {name || salary || avatarUrl ? (
        <Box
          p={8}
          borderWidth="1px"
          borderRadius="lg"
          overflow="hidden"
          width="auto"
          maxW="auto"
          bg="white"
          boxShadow="sm"
          textAlign="center"
        >
          <Stack direction="row" align="center">
            {/* Avatar */}
            <Box bg={`#ffc80820`} p={4} borderRadius="lg" marginRight="30%">
              <Avatar src={avatarUrl} size="xl" />
            </Box>

            {/* Info */}
            <Box textAlign="left">
              <Text fontSize="lg" color="gray.500">
                {name}
              </Text>
              <Text fontSize="3xl" fontWeight="bold">
                ${salary}
              </Text>
            </Box>
          </Stack>
        </Box>
      ) : null}
    </Box>
  );
}
